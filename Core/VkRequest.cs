using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using VkLib.Error;

namespace VkLib.Core
{
    /// <summary>
    /// An object that performs request to vk server
    /// </summary>
    internal class VkRequest
    {
        internal static string UserAgent { get; set; }

        public static async Task<JObject> GetAsync(string url, Dictionary<string, string> parameters)
        {
            var uri = new Uri(url);
            var fullUri = GetFullUri(uri, parameters);

            Debug.WriteLine($"VK GET {fullUri}");

            var httpClient = GetHttpClient();

            HttpResponseMessage responseMessage = await httpClient.GetAsync(fullUri);
            var content = await responseMessage.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(content))
            {
                Debug.WriteLine($"VK Response {content}");

                var response = JObject.Parse(content);

                try
                {
                    VkErrorProcessor.ProcessError(response);
                }
                catch (VkTooManyRequestsException)
                {
                    Debug.WriteLine("VK Request throttling");

                    //wait for 3 secs
                    await Task.Delay(3000);
                    return await GetAsync(url, parameters);
                }

                return response;
            }

            return null;
        }

        public static async Task<JObject> PostAsync(string url, Dictionary<string, string> parameters)
        {
            var uri = new Uri(url);
            Debug.WriteLine($"VK POST {uri}");

            var httpClient = GetHttpClient();

            var postContent = new FormUrlEncodedContent(parameters);
            postContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            HttpResponseMessage responseMessage = await httpClient.PostAsync(uri, postContent);
            var content = await responseMessage.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(content))
            {
                var response = JObject.Parse(content);

                VkErrorProcessor.ProcessError(response);

                return response;
            }

            return null;
        }

        private static Uri GetFullUri(Uri baseUri, Dictionary<string, string> parameters)
        {
            if (parameters != null && parameters.Count > 0)
            {
                var paramStr = string.Join("&", parameters.Select(kp => $"{Uri.EscapeDataString(kp.Key)}={Uri.EscapeDataString(kp.Value)}"));

                return new Uri(string.Concat(baseUri, "?", paramStr));
            }

            return baseUri;
        }

        private static HttpClient GetHttpClient()
        {
            var handler = new HttpClientHandler();
            if (handler.SupportsAutomaticDecompression)
            {
                handler.AutomaticDecompression = DecompressionMethods.GZip |
                                                 DecompressionMethods.Deflate;
            }

            var client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("User-Agent", UserAgent);

            return client;
        }
    }
}
