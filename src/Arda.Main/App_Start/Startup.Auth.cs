using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Arda.Common.Utils;
using Arda.Main.Utils;
using System.Net.Http;

namespace Arda.Main
{
    public partial class Startup
    {
        public static string Authority = string.Empty;
        public static string CallbackPath = string.Empty;
        public static string ClientId = string.Empty;
        public static string ClientSecret = string.Empty;
        public static string GraphResourceId = string.Empty;
        public static string PostLogoutRedirectUri = string.Empty;

        public void ConfigureAuth(IApplicationBuilder app)
        {
            // Populate Azure AD Configuration Values
            Authority = Configuration["Authentication:AzureAd:AADInstance"] + Configuration["Authentication:AzureAd:TenantId"];
            CallbackPath = Configuration["Authentication:AzureAd:CallbackPath"];
            ClientId = Configuration["Authentication:AzureAd:ClientId"];
            ClientSecret = Configuration["Authentication:AzureAd:ClientSecret"];
            GraphResourceId = Configuration["Authentication:AzureAd:GraphResourceId"];
            PostLogoutRedirectUri = Configuration["Authentication:AzureAd:PostLogoutRedirectUri"];

            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AutomaticAuthenticate = true
            });

            if( ClientId == null || ClientId == "" )
            {
                // quick return
                return;
            }

            app.UseOpenIdConnectAuthentication(new OpenIdConnectOptions()
            {
                AutomaticChallenge = true,
                CallbackPath = new PathString(CallbackPath),
                ClientId = ClientId,
                Authority = Authority,
                PostLogoutRedirectUri = PostLogoutRedirectUri,
                SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme,

                Events = new OpenIdConnectEvents()
                {
                    OnAuthenticationFailed = OnAuthenticationFailed,
                    OnAuthorizationCodeReceived = OnAuthorizationCodeReceived
                }
            });
        }


        public Task OnAuthenticationFailed(AuthenticationFailedContext context)
        {
            Debugger.Break();
            context.HandleResponse();
            context.Response.Redirect("/Home/Error?message=" + context.Exception.Message);

            return Task.FromResult(0);
        }

        public async Task OnAuthorizationCodeReceived(AuthorizationCodeReceivedContext context)
        {
            //await CacheUserAndCodeOnRedis(context);
            await AcquireTokenForMicrosoftGraph(context);
        }


        private async Task CacheUserAndCodeOnRedis(AuthorizationCodeReceivedContext context)
        {
            var claims = context.JwtSecurityToken.Claims;

            // Getting informations about AD
            var code = context.JwtSecurityToken.Id;
            var validFrom = context.JwtSecurityToken.ValidFrom;
            var validTo = context.JwtSecurityToken.ValidTo;
            var givenName = claims.FirstOrDefault(claim => claim.Type == "given_name").Value;
            var name = claims.FirstOrDefault(claim => claim.Type == "name").Value;
            var uniqueName = claims.FirstOrDefault(claim => claim.Type == "unique_name").Value;

            await Util.ConnectToRemoteService(HttpMethod.Post, Util.PermissionsURL + "api/permission/setuserpermissionsandcode?name=" + name, uniqueName, code);
        }

        private async Task AcquireTokenForMicrosoftGraph(AuthorizationCodeReceivedContext context)
        {
            // Acquire a Token for the Graph API and cache it in Session.
            string userObjectId = context.Ticket.Principal.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            ClientCredential clientCred = new ClientCredential(ClientId, ClientSecret);
            AuthenticationContext authContext = new AuthenticationContext(Authority, new SessionCache(userObjectId, context.HttpContext));
            AuthenticationResult authResult = await authContext.AcquireTokenByAuthorizationCodeAsync(
                context.JwtSecurityToken.Id, new Uri(context.ProtocolMessage.RedirectUri), clientCred, GraphResourceId);
        }
    }
}