using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Arda.Common.Utils;
using Arda.Main.Utils;
using System.Net.Http;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Arda.Main
{
    public partial class Startup
    {
        public static bool IsSimpleAuthForDemo;
        public static string Authority = string.Empty;
        public static string CallbackPath = string.Empty;
        public static string ClientId = string.Empty;
        public static string ClientSecret = string.Empty;
        public static string GraphResourceId = string.Empty;
        public static string PostLogoutRedirectUri = string.Empty;
        private static string _dbgAccessToken = null;
        private static string _dbgTokenId = null;

        public void ConfigureAuth(IApplicationBuilder app)
        {
            // Populate Azure AD Configuration Values
            Authority = Configuration.Get("Authentication_AzureAd_AADInstance") + Configuration.Get("Authentication_AzureAd_TenantId");
            CallbackPath = Configuration.Get("Authentication_AzureAd_CallbackPath");
            ClientId = Configuration.Get("Authentication_AzureAd_ClientId");
            ClientSecret = Configuration.Get("Authentication_AzureAd_ClientSecret");
            GraphResourceId = Configuration.Get("Authentication_AzureAd_GraphResourceId");
            PostLogoutRedirectUri = Configuration.Get("Authentication_AzureAd_PostLogoutRedirectUri");            

            IsSimpleAuthForDemo = (ClientId == null || ClientId == "");

            if ( IsSimpleAuthForDemo )
            {
                app.UseCookieAuthentication(new CookieAuthenticationOptions()
                {
                    AuthenticationScheme = "defaultCookieAuth",
                    LoginPath = new PathString("/Account/Unauthorized/"),
                    AccessDeniedPath = new PathString("/Account/Forbidden/"),
                    AutomaticAuthenticate = true,
                    AutomaticChallenge = true
                });                

                // quick return
                return;
            }
            
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AutomaticAuthenticate = true
            });

            app.UseOpenIdConnectAuthentication(new OpenIdConnectOptions()
            {
                AutomaticChallenge = true,
                CallbackPath = new PathString(CallbackPath),
                ClientId = ClientId,
                Authority = Authority,
                PostLogoutRedirectUri = PostLogoutRedirectUri,
                SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme,
                ResponseType = OpenIdConnectResponseType.CodeIdToken,

                Events = new OpenIdConnectEvents()
                {
                    OnAuthenticationFailed = OnAuthenticationFailed,
                    OnAuthorizationCodeReceived = OnAuthorizationCodeReceived,
                    OnMessageReceived = (MessageReceivedContext m) => {
                        return Task.FromResult(0);  }
                }
            });            
        }


        public Task OnAuthenticationFailed(AuthenticationFailedContext context)
        {
            context.HandleResponse();
            context.Response.Redirect("/Home/Error?message=" + context.Exception.Message);

            return Task.FromResult(0);
        }

        public async Task OnAuthorizationCodeReceived(AuthorizationCodeReceivedContext context)
        {
            // temporarily remove this code
            //await CacheUserAndCodeOnRedis(context);
            await AcquireTokenForMicrosoftGraph(context);
        }


        private async Task CacheUserAndCodeOnRedis(AuthorizationCodeReceivedContext context)
        {
            var claims = context.JwtSecurityToken.Claims;

            // Getting informations about AD
            var code = context.JwtSecurityToken.Id;
            //var validFrom = context.JwtSecurityToken.ValidFrom;
            //var validTo = context.JwtSecurityToken.ValidTo;
            //var givenName = claims.FirstOrDefault(claim => claim.Type == "given_name").Value;
            var name = claims.FirstOrDefault(claim => claim.Type == "name").Value;
            var uniqueName = claims.FirstOrDefault(claim => claim.Type == "unique_name").Value;

            // TODO: remove this code. we should NOT call permissions API from inside Auth process!
            await Util.ConnectToRemoteService(HttpMethod.Post, Util.PermissionsURL + "api/permission/setuserpermissionsandcode?name=" + name, uniqueName, code);
        }

        private async Task AcquireTokenForMicrosoftGraph(AuthorizationCodeReceivedContext context)
        {
            //Uri callback = Util.MainURL + CallbackPath;
            
            // Acquire a Token for the Graph API and cache it in Session.
            string userObjectId = context.Ticket.Principal.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            ClientCredential clientCred = new ClientCredential(ClientId, ClientSecret);
            AuthenticationContext authContext = new AuthenticationContext(Authority, new SessionCache(userObjectId, context.HttpContext));

            // Per sample: https://github.com/Azure-Samples/active-directory-dotnet-webapp-webapi-openidconnect-aspnetcore/blob/master/WebApp-WebAPI-OpenIdConnect-DotNet/Startup.cs
            AuthenticationResult authResult = await authContext.AcquireTokenByAuthorizationCodeAsync(
                 context.ProtocolMessage.Code, new Uri(context.Properties.Items[OpenIdConnectDefaults.RedirectUriForCodePropertiesKey]), clientCred, GraphResourceId);
            
            // -- See https://github.com/aspnet/Security/issues/1068
            context.HandleCodeRedemption(authResult.AccessToken, authResult.IdToken);

            _dbgAccessToken = authResult.AccessToken;
            _dbgTokenId = authResult.IdToken;
        }
    }
}