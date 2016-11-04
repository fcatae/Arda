using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Arda.Common.Models;
using Arda.Common.Utils;

namespace Arda.Common.Middlewares
{
    public class SecurityAPIMiddleware
    {
        RequestDelegate _next;

        public SecurityAPIMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                var user = context.Request.Headers["unique_name"].ToString();
                var code = context.Request.Headers["code"].ToString();
                //TODO: Compare with Cache

                var endpoint = context.Request.Host.Value;
                var path = context.Request.Path.ToString();

                var temp = path.Remove(0, "/api/".Length).Split('/');
                var module = temp[0];
                var resource = temp[1];

                if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(module) || string.IsNullOrWhiteSpace(resource))
                {
                    //Bad Request:
                    context.Response.StatusCode = 400;
                    return;
                }
                else if (!CheckUserPermissionToResource(user, module, resource))
                {
                    //User doesn't have permission, code is not valid or code is expired:
                    context.Response.StatusCode = 401;
                    return;
                }
                else
                {
                    await _next(context);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool CheckUserPermissionToResource(string uniqueName, string module, string resource)
        {
            //TODO: Refactor security
            return true;

            //var client = new HttpClient();
            //client.BaseAddress = new Uri(Util.PermissionsURL + "api/");

            //string url = client.BaseAddress + string.Format("permission/verifyuseraccesstoresource?uniquename={0}&module={1}&resource={2}", uniqueName, module, resource);
            //var response = client.GetAsync(url).Result;

            //var bodySerialized = response.Content.ReadAsStringAsync().Result;
            //var bodyResponse = JsonConvert.DeserializeObject<HTTPBodyResponse>(bodySerialized);

            //if (bodyResponse.IsSuccessStatusCode)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }
    }
}
