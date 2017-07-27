using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Threading.Tasks;

namespace Arda.Main.Utils
{
    [System.Obsolete("Dont use", false)]
    public static class TokenManager
    {
        public static async Task<AuthenticationResult> GetAccessToken(HttpContext context)
        {
            string userObjectID = context.User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var tokenCache = new NaiveSessionCache(userObjectID, NaiveSessionCacheResource.MicrosoftGraph, context.Session);
            ClientCredential credential = new ClientCredential(Startup.ClientId, Startup.ClientSecret);
            AuthenticationContext authContext = new AuthenticationContext(Startup.Authority, tokenCache);
            
            //return await authContext.AcquireTokenAsync(Startup.GraphResourceId, credential);

            var result = await authContext.AcquireTokenSilentAsync(Startup.GraphResourceId, credential, new UserIdentifier(userObjectID, UserIdentifierType.UniqueId));
            
            return result;
        }

        //private static async Task<IActionResult> CallMicrosoftGraph(HttpContext context)
        //{
        //    AuthenticationResult result = null;

        //    try
        //    {
        //        string userObjectID = context.User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
        //        AuthenticationContext authContext = new AuthenticationContext(Startup.Authority, new SessionCache(userObjectID, context));
        //        ClientCredential credential = new ClientCredential(Startup.ClientId, Startup.ClientSecret);
        //        result = await authContext.AcquireTokenSilentAsync(Startup.GraphResourceId, credential, new UserIdentifier(userObjectID, UserIdentifierType.UniqueId));

        //        return null;
        //    }
        //    catch (Exception)
        //    {
        //        //If get silent token fails: retry the auth
        //        return new ChallengeResult(OpenIdConnectDefaults.AuthenticationScheme);
        //    }
        //}
    }
}
