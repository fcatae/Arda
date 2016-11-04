using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arda.Main.Utils
{
    public static class TokenManager
    {
        public static async Task<AuthenticationResult> GetAccessToken(HttpContext context)
        {
            string userObjectID = context.User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            AuthenticationContext authContext = new AuthenticationContext(Startup.Authority, new SessionCache(userObjectID, context));
            ClientCredential credential = new ClientCredential(Startup.ClientId, Startup.ClientSecret);

            return await authContext.AcquireTokenAsync(Startup.GraphResourceId, credential);
            
            // Codigo antigo
            //return await authContext.AcquireTokenSilentAsync(Startup.GraphResourceId, credential, new UserIdentifier(userObjectID, UserIdentifierType.UniqueId));

        }

        private static async Task<IActionResult> CallMicrosoftGraph(HttpContext context)
        {
            AuthenticationResult result = null;

            try
            {
                string userObjectID = context.User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
                AuthenticationContext authContext = new AuthenticationContext(Startup.Authority, new SessionCache(userObjectID, context));
                ClientCredential credential = new ClientCredential(Startup.ClientId, Startup.ClientSecret);
                result = await authContext.AcquireTokenSilentAsync(Startup.GraphResourceId, credential, new UserIdentifier(userObjectID, UserIdentifierType.UniqueId));
                return null;
                //HttpClient client = new HttpClient();
                //HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/users/maluz@microsoft.com/photo/$value");
                //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
                //HttpResponseMessage response = await client.SendAsync(request);

                //if (response.IsSuccessStatusCode)
                //{
                //    var source = await response.Content.ReadAsByteArrayAsync();
                //    return View();
                //}
                //else
                //{
                //    //
                //    // If the call failed with access denied, then drop the current access token from the cache,
                //    //     and show the user an error indicating they might need to sign-in again.
                //    //
                //    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                //    {

                //        //var todoTokens = authContext.TokenCache.ReadItems().Where(a => a.Resource == Startup.TodoListResourceId);
                //        //foreach (TokenCacheItem tci in todoTokens)
                //        //    authContext.TokenCache.DeleteItem(tci);

                //        //ViewBag.ErrorMessage = "UnexpectedError";
                //    }
                //}
            }
            catch (Exception)
            {
                //If get silent token fails:
                return new ChallengeResult(OpenIdConnectDefaults.AuthenticationScheme);
            }


            //return View("Error");
        }
    }
}
