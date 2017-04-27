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
using Microsoft.ApplicationInsights;

namespace Arda.Common.Utils
{
    public static class Util
    {
        // Environment
        public static string KanbanURL;
        public static string MainURL;
        public static string ReportsURL;
        public static string PermissionsURL;

        private static IDistributedCache _cacheDontUse;

        static Util()
        {

        }

        public static string GetUserPhotoFromRemote(string user)
        {
            try
            {
                // Sometimes we have a flood of requests (needs caching)
                // Sometimes caching fails because the first request fails - needs to check permissions API
                var photo = ConnectToRemoteServiceString(HttpMethod.Get, PermissionsURL + "api/permission/getuserphotofromcache?uniqueName=" + user, user, "").Result;

                return photo;
            }
            catch(AggregateException exc)
            {
                if(exc.InnerException is TaskCanceledException)
                {
                    var currentException = exc.InnerException;

                    var client = new TelemetryClient();
                    client.TrackException(currentException);

                    throw currentException;
                }

                throw exc;
            }
        }


        public static string GetUserPhoto(string user)
        {
            if (user == "undefined") throw new Exception("GetUserPhoto: user is undefined");

            //var key = "photo_" + user;
            string arrPicture = null;

            try
            {
                //Try from Cache:
                arrPicture = GetUserPhotoFromRemote(user); // _cache.Get(key);

                if( arrPicture != null )
                {
                    var photo = arrPicture; // Util.GetString(arrPicture);
                    return photo;
                }
            }
            catch (StackExchange.Redis.RedisConnectionException)
            {
                // Ignore transient network errors
            }

            //Try from DB:

            try
            {
                // Getting the response of remote service
                var response = ConnectToRemoteService(HttpMethod.Put, PermissionsURL + "api/permission/saveuserphotooncache?=" + user, user, "").Result;
                if (response.IsSuccessStatusCode)
                {
                    arrPicture = GetUserPhotoFromRemote(user); // _cache.Get(key);

                    var photo = arrPicture; // Util.GetString(_cache.Get(key));
                    return photo;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch(AggregateException exc)
            {
                if( exc.InnerException is JsonReaderException)
                {
                    var currentException = exc.InnerException as JsonReaderException;

                    throw new Exception("api/permission/saveuserphotooncache: failed with JsonReaderException", currentException);
                }
                // may raise JsonReaderException -- when the API fails to find the corresponding picture
                throw exc;
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

        public static async Task<string> ClientSendAsync(HttpMethod method, string url, string uniqueName, string code)
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(method, url);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                request.Headers.Add("unique_name", uniqueName);
                request.Headers.Add("code", code);

                var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    throw new InvalidOperationException("Invalid HTTP code: " + response.StatusCode);
                }

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception httpExc)
            {
                throw new Exception("Could not contact endpoint: " + url, httpExc);
            }
        }

        public static async Task<T> ConnectToRemoteService<T>(HttpMethod method, string url, string uniqueName, string code)
        {
            var responseJson = await ClientSendAsync(method, url, uniqueName, code);

            var responseConverted = JsonConvert.DeserializeObject<T>(responseJson);

            return responseConverted;
        }

        public static async Task<string> ConnectToRemoteServiceString(HttpMethod method, string url, string uniqueName, string code)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(method, url);

            request.Headers.Add("unique_name", uniqueName);
            request.Headers.Add("code", code);

            var responseSend = await client.SendAsync(request);

            if (responseSend.IsSuccessStatusCode)
            {
                var responseStr = await responseSend.Content.ReadAsStringAsync();

                if (responseStr != "")
                    return responseStr;
            }

            return null;
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
            MainURL = config["Endpoints:ardaapp"];
            PermissionsURL = config["Endpoints:permissions-service"];
            KanbanURL = config["Endpoints:kanban-service"];
            ReportsURL = config["Endpoints:reports-service"];            
        }
    }
}