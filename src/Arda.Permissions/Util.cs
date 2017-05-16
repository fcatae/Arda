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
using StackExchange.Redis;
using System.Linq;
using System.Text.RegularExpressions;

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
        private static ConnectionMultiplexer _redis;
        private static IDatabase DB;

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
                //arrPicture = _cache.Get(key);

                if( arrPicture != null )
                {
                    var photo = GetValue(key);
                    //var photo = Util.GetString(arrPicture);
                    
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
                var photo = GetValue(key); // Util.GetString(_cache.Get(key));
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
            MainURL = config["Endpoint_ardaapp"];
            PermissionsURL = config["Endpoints_permissions_service"];
            KanbanURL = config["Endpoints_kanban_service"];
            ReportsURL = config["Endpoints_reports_service"];


            var options = new RedisCacheOptions
            {
                Configuration = config["Storage_Redis_Configuration"],
                InstanceName = config["Storage_Redis_InstanceName"]
            };

            Console.WriteLine("antes de alterar: options: " + options.Configuration + "\r\n");
            Util.ResolveDns(options);
            Console.WriteLine("Depois de alterar: options: " + options.Configuration + "\r\n");

            _redis = ConnectionMultiplexer.Connect(options.Configuration);

            _cache = new RedisCache(options);
        }

        public static void ResolveDns(this RedisCacheOptions options)
        {
            // Assume that the first part is host and port.
            var hostWithPort = options.Configuration.Substring(0, options.Configuration.IndexOf(","));
            var resolved = TryResolveDns(hostWithPort);
            var replaced = options.Configuration.Replace(hostWithPort, resolved);
            options.Configuration = replaced;
        }

        private static string TryResolveDns(string redisUrl)
        {
            var config = ConfigurationOptions.Parse(redisUrl);

            foreach (DnsEndPoint addressEndpoint in config.EndPoints)
            {
                var port = addressEndpoint.Port;
                var isIp = IsIpAddress(addressEndpoint.Host);
                if (!isIp)
                {
                    var ip = Dns.GetHostEntryAsync(addressEndpoint.Host).GetAwaiter().GetResult();
                    return $"{ip.AddressList.First(x => IsIpAddress(x.ToString()))}:{port}";
                }
            }

            return redisUrl;
        }

        private static bool IsIpAddress(string host)
        {
            return Regex.IsMatch(host, @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");
        }


        private static void GetDBInstance()
        {
            if (DB == null)
            {
                DB = _redis.GetDatabase();
            }
        }
        public static bool SetValue(string key, string value)
        {
            GetDBInstance();
            return DB.StringSet(key, value);
        }
        public static string GetValue(string key)
        {
            GetDBInstance();
            return DB.StringGet(key);
        }

    }
}