using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Configuration;

namespace Arda.Common.Utils
{
    public static class Util
    {
        // Environment
        public static string KanbanURL;
        public static string MainURL;
        public static string ReportsURL;
        public static string PermissionsURL;

        private static IDistributedCache _cache;

        static Util()
        {

        }

        public static string GetUserPhoto(string user)
        {
            var key = "photo_" + user;
            byte[] arrPicture = null;

            try
            {
                //Try from Cache:
                arrPicture = _cache.Get(key);

                if( arrPicture != null )
                {
                    var photo = Util.GetString(arrPicture);
                    return photo;
                }
            }
            catch (StackExchange.Redis.RedisConnectionException)
            {
                // Ignore transient network errors
            }

            //Try from DB:

            // Getting the response of remote service
            var response = ConnectToRemoteService(HttpMethod.Put, PermissionsURL + "api/permission/saveuserphotooncache?=" + user, user, "").Result;
            if (response.IsSuccessStatusCode)
            {
                var photo = Util.GetString(_cache.Get(key));
                return photo;
            }
            else
            {
                return string.Empty;
            }
        }

        public static byte[] GetBytes(string obj)
        {
            return Encoding.UTF8.GetBytes(obj);
        }

        public static string GetString(byte[] obj)
        {
            return Encoding.UTF8.GetString(obj);
        }

        public static async Task<T> ConnectToRemoteService<T>(HttpMethod method, string url, string uniqueName, string code)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(method, url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            request.Headers.Add("unique_name", uniqueName);
            request.Headers.Add("code", code);

            var responseRaw = await client.SendAsync(request);
            var responseJson = responseRaw.Content.ReadAsStringAsync().Result;
            var responseConverted = JsonConvert.DeserializeObject<T>(responseJson);

            return responseConverted;
        }

        public static async Task<HttpResponseMessage> ConnectToRemoteService(HttpMethod method, string url, string uniqueName, string code)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(method, url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add("unique_name", uniqueName);
            request.Headers.Add("code", code);

            var responseSend = await client.SendAsync(request);
            var responseStr = await responseSend.Content.ReadAsStringAsync();
            var responseObj = JsonConvert.DeserializeObject<HttpResponseMessage>(responseStr);

            if (responseSend.IsSuccessStatusCode && responseObj.IsSuccessStatusCode)
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        public static async Task<HttpResponseMessage> ConnectToRemoteService<T>(HttpMethod method, string url, string uniqueName, string code, T body)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(method, url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add("unique_name", uniqueName);
            request.Headers.Add("code", code);

            var serialized = JsonConvert.SerializeObject(body);
            request.Content = new ByteArrayContent(GetBytes(serialized));

            var responseSend = await client.SendAsync(request);
            var responseStr = await responseSend.Content.ReadAsStringAsync();
            var responseObj = JsonConvert.DeserializeObject<HttpResponseMessage>(responseStr);

            if (responseSend.IsSuccessStatusCode && responseObj.IsSuccessStatusCode)
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        public static async Task<HttpResponseMessage> ConnectToRemoteService<T>(HttpMethod method, string url, T body)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(method, url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (body != null)
            {
                var serialized = JsonConvert.SerializeObject(body);
                request.Content = new ByteArrayContent(GetBytes(serialized));
            }

            var responseSend = await client.SendAsync(request);
            var responseStr = await responseSend.Content.ReadAsStringAsync();
            var responseObj = JsonConvert.DeserializeObject<HttpResponseMessage>(responseStr);

            if (responseSend.IsSuccessStatusCode && responseObj.IsSuccessStatusCode)
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        public static async Task<HttpResponseMessage> ConnectToRemoteService<T>(HttpMethod method, string url, string uniqueName, string code, Guid id)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(method, url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add("unique_name", uniqueName);
            request.Headers.Add("code", code);

            var responseSend = await client.SendAsync(request);
            var responseStr = await responseSend.Content.ReadAsStringAsync();
            var responseObj = JsonConvert.DeserializeObject<HttpResponseMessage>(responseStr);

            if (responseSend.IsSuccessStatusCode && responseObj.IsSuccessStatusCode)
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        public static Guid GenerateNewGuid()
        {
            return Guid.NewGuid();
        }

        public static string GetUserAlias(string uniqueName)
        {
            string result = uniqueName.Split('@')[0];
            return result;
        }

        public static void SetEnvironmentVariables(IConfiguration config)
        {
            MainURL = config.Get("Endpoints_ardaapp");
            PermissionsURL = config.Get("Endpoints_permissions_service");
            KanbanURL = config.Get("Endpoints_kanban_service");
            ReportsURL = config.Get("Endpoints_reports_service");


            _cache = new RedisCache(new RedisCacheOptions
            {
                Configuration = config.Get("Storage_Redis_Configuration"),
                InstanceName = config.Get("Storage_Redis_InstanceName")
            });
        }
        
        public static string Get(this IConfiguration config, string name)
        {
            string tenantVal = GetTenantConfiguration(config, name);

            string val1 = config[name];
            string val2 = config[Rename(name)];

            return tenantVal ?? val1 ?? val2;
        }

        public static string Get(this IConfigurationRoot config, string name)
        {
            string tenantVal = GetTenantConfiguration(config, name);

            string val1 = config[name];
            string val2 = config[Rename(name)];

            return tenantVal ?? val1 ?? val2;
        }

        static string GetTenantConfiguration(IConfiguration config, string name)
        {
            if(config["ArdaTenantId"] != null)
            {
                if( name.Contains("SqlServer_Kanban"))
                {
                    string ardaTenant = config["ArdaTenantId"];
                    var comp = ardaTenant.Split(':');
                    string tenantServer = comp[0];
                    string tenantId = comp[1];

                    var client = new HttpClient();
                    var request = new HttpRequestMessage(HttpMethod.Get, $"http://{tenantServer}/database/{tenantId}/config");
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var responseSend = client.SendAsync(request).Result;
                    var responseStr = responseSend.Content.ReadAsStringAsync().Result;

                    dynamic responseObj = JsonConvert.DeserializeObject(responseStr);

                    return responseObj.kanban_Database; // Storage_SqlServer_Kanban_Connection_String;
                }
            }

            return null;
        }

        static string Rename(string name)
        {
            string current = name;

            if (name.Contains("SqlServer_"))
                current = name.Replace("SqlServer_", "SqlServer-");

            if (name.Contains("Endpoint_"))
                current = name.Replace("Endpoint_", "Endpoint:");

            if (name.EndsWith("_service"))
                current = name.Replace("_service", "-service");

            current = current.Replace("_", ":");

            return current;
        }
    }

}