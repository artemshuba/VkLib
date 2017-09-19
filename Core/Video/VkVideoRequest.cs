using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using VkLib.Core.Audio;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;

namespace VkLib.Core.Video
{
    public class VkVideoRequest
    {
        private const int MAX_VIDEO_COUNT = 200;

        private readonly Vk _vkontakte;

        internal VkVideoRequest(Vk vkontakte)
        {
            _vkontakte = vkontakte;
        }

        public async Task<VkItemsResponse<VkVideo>> Get(IList<string> videos, string ownerId = null, string albumId = null, int count = 0, int offset = 0, bool extended = false)
        {
            if (count > 200)
                throw new ArgumentException("Maximum count is 200.");

            var parameters = new Dictionary<string, string>();

            if (videos != null)
                parameters.Add("videos", string.Join(",", videos));

            if (!string.IsNullOrEmpty(ownerId))
                parameters.Add("owner_id", ownerId);

            if (!string.IsNullOrEmpty(albumId))
                parameters.Add("album_id", albumId);

            if (count > 0)
                parameters.Add("count", count.ToString(CultureInfo.InvariantCulture));
            else
                parameters.Add("count", MAX_VIDEO_COUNT.ToString(CultureInfo.InvariantCulture));

            if (offset > 0)
                parameters.Add("offset", offset.ToString(CultureInfo.InvariantCulture));

            if (extended)
                parameters.Add("extended", "1");

            _vkontakte.SignMethod(parameters);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "video.get", parameters);

            if (response.SelectToken("response.items") != null)
                return new VkItemsResponse<VkVideo>(response["response"]["items"].Select(VkVideo.FromJson).ToList(), (int)response["response"]["count"]);

            return null;
        }

        public async Task<IEnumerable<VkVideo>> Search(string query, int count = 0, int offset = 0, bool hdOnly = false, VkAudioSortType sort = VkAudioSortType.DateAdded, bool adult = false)
        {
            if (count > 200)
                throw new ArgumentException("Maximum count is 200.");

            if (query == null)
                throw new ArgumentException("Query must not be null.");

            var parameters = new Dictionary<string, string>();

            parameters.Add("q", query);

            if (hdOnly)
                parameters.Add("hd", "1");

            parameters.Add("sort", ((int)sort).ToString(CultureInfo.InvariantCulture));

            if (adult)
                parameters.Add("adult", "1");

            if (count > 0)
                parameters.Add("count", count.ToString(CultureInfo.InvariantCulture));
            else
                parameters.Add("count", MAX_VIDEO_COUNT.ToString(CultureInfo.InvariantCulture));

            if (offset > 0)
                parameters.Add("offset", offset.ToString(CultureInfo.InvariantCulture));

            parameters.Add("access_token", _vkontakte.AccessToken.Token);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "video.search", parameters);

            if (response["response"]?.HasValues == true)
            {
                return from v in response["response"] select VkVideo.FromJson(v);
            }

            return null;
        }

        public async Task<VkVideoSaveResponse> Save(string name = null, string description = null, bool isPrivate = false)
        {
            var parameters = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(name))
                parameters.Add("name", name);

            if (!string.IsNullOrEmpty(description))
                parameters.Add("description", description);

            if (isPrivate)
                parameters.Add("is_private", "1");

            _vkontakte.SignMethod(parameters);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "video.save", parameters);

            if (response["response"] != null)
                return VkVideoSaveResponse.FromJson(response["response"]);

            return null;
        }

        public async Task<VkUploadVideoResponse> Upload(string url, string fileName, Stream stream)
        {
            var client = new HttpClient();

            string boundary = "----------" + DateTime.Now.Ticks.ToString("x");
            var content = new MultipartFormDataContent(boundary);

            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                FileName = "\"" + fileName + "\"",
                Name = "\"video_file\""
            };

            content.Add(fileContent);

            var responseMessage = await client.PostAsync(new Uri(url), content);
            byte[] bytes = await responseMessage.Content.ReadAsByteArrayAsync();

            Encoding encoding = Encoding.UTF8; //VK return windows-1251 which causes exception on Win10
            string response = encoding.GetString(bytes, 0, bytes.Length);

            Debug.WriteLine("Upload response: " + response);

            var json = JObject.Parse(response);
            return VkUploadVideoResponse.FromJson(json);
        }
    }
}
