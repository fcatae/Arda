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
using System.IO;
using System.Net;
using Microsoft.Extensions.WebEncoders;

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

            if (IsSimpleAuthForDemo)
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

            OpenIdConnectOptions openidOptions = new OpenIdConnectOptions()
            {
                AutomaticChallenge = true,
                CallbackPath = new PathString(CallbackPath),
                ClientId = ClientId,
                Authority = Authority,
                PostLogoutRedirectUri = PostLogoutRedirectUri,
                SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme,
                ResponseType = OpenIdConnectResponseType.CodeIdToken,
                Resource = "https://graph.microsoft.com/",

                Events = new OpenIdConnectEvents()
                {
                    OnAuthenticationFailed = OnAuthenticationFailed,
                    OnAuthorizationCodeReceived = OnAuthorizationCodeReceived,
                    OnMessageReceived = (MessageReceivedContext m) =>
                    {
                        return Task.FromResult(0);
                    }
                }
            };

            app.UseOpenIdConnectAuthentication(openidOptions);
        }


        public Task OnAuthenticationFailed(AuthenticationFailedContext context)
        {
            context.HandleResponse();

            context.Response.Redirect("/Home/Error?message=Authentication falilure&oauth=true");
            Debug.WriteLine(context.Exception.Message);

            return Task.CompletedTask;
        }

        public async Task OnAuthorizationCodeReceived(AuthorizationCodeReceivedContext context)
        {
            await AcquireTokenForMicrosoftGraph(context);

        }


        private async Task AcquireTokenForMicrosoftGraph(AuthorizationCodeReceivedContext context)
        {

            string userObjectID = context.Ticket.Principal.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            AuthenticationContext authContext  = new AuthenticationContext(Authority, new NaiveSessionCache(userObjectID, NaiveSessionCacheResource.MicrosoftGraph ,context.HttpContext.Session));

            // Acquire a Token for the Graph API and cache it in Session.
            ClientCredential clientCred = new ClientCredential(ClientId, ClientSecret);

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