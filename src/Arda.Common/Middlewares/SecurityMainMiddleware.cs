using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arda.Common.Middlewares
{
    public class SecurityMainMiddleware
    {
        RequestDelegate _next;
        List<Tuple<string, string>> defaultResources;

        public SecurityMainMiddleware(RequestDelegate next)
        {
            _next = next;
            defaultResources = new List<Tuple<string, string>>();
            defaultResources.Add(Tuple.Create(string.Empty, string.Empty));
            defaultResources.Add(Tuple.Create("Dashboard", string.Empty));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                //1st: If the user is not authenticated, the [Authorize] Annotation takes cares:
                if (!context.User.Identity.IsAuthenticated)
                {
                    //RETURN ==> Requested Page:
                    await _next(context);
                    return;
                }
                //Get user info:
                var uniqueName = context.User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
                //Get the requested page info:
                string path = context.Request.Path.ToString().Substring(1);
                string module;
                string resource;
                var temp = path.Split('/');
                if (temp.Length == 2)
                {
                    module = temp[0];
                    resource = temp[1];
                }
                else if (temp.Length == 1)
                {
                    module = temp[0];
                    resource = "";
                }
                else
                {
                    //RETURN ==> Bad Request:
                    context.Response.StatusCode = 400;
                    return;
                }

                //2nd: Is this module/resource a defaultResource?
                if (defaultResources.Contains(Tuple.Create(module, resource)))
                {
                    //RETURN -> Requested Page:
                    await _next(context);
                    return;
                }
                //3rd: Does user have permissions to access it?
                else if (checkMainPermission(uniqueName, module, resource))
                {

                }
                //Not allowed:
                else
                {
                    //RETURN ==> Bad Request:
                    context.Response.StatusCode = 403;
                    return;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        bool checkMainPermission(string uniqueName, string module, string resource)
        {
            //TODO: Call Redis Cache and check if the user has permission
            return true;
        }
    }
}
