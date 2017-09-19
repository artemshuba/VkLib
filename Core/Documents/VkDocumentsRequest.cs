using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using VkLib.Core.Attachments;
using VkLib.Core.Photos;

namespace VkLib.Core.Documents
{
    public class VkDocumentsRequest
    {
        private readonly Vk _vk;

        public VkDocumentsRequest(Vk vk)
        {
            _vk = vk;
        }

        public async Task<string> GetUploadServer()
        {
            var parameters = new Dictionary<string, string>();

            _vk.SignMethod(parameters);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "docs.getUploadServer", parameters);

            return response["response"]["upload_url"]?.Value<string>();
        }

        public async Task<VkUploadDocumentResponse> Upload(string url, string fileName, Stream docStream)
        {
            var client = new HttpClient();

            string boundary = "----------" + DateTime.Now.Ticks.ToString("x");
            var content = new MultipartFormDataContent(boundary);

            var fileContent = new StreamContent(docStream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                FileName = "\"" + fileName + "\"",
                Name = "\"file\""
            };

            content.Add(fileContent);

            var responseMessage = await client.PostAsync(new Uri(url), content);
            byte[] bytes = await responseMessage.Content.ReadAsByteArrayAsync();

            Encoding encoding = Encoding.UTF8; //VK return windows-1251 which causes exception on Win10
            string response = encoding.GetString(bytes, 0, bytes.Length);

            Debug.WriteLine("VkLib upload response: " + response);

            var json = JObject.Parse(response);
            return VkUploadDocumentResponse.FromJson(json);
        }

        public async Task<VkDocumentAttachment> Save(string file, string title = null, string tags = null)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("file", file);

            if (!string.IsNullOrEmpty(title))
                parameters.Add("title", title);

            if (!string.IsNullOrEmpty(tags))
                parameters.Add("tags", tags);

            _vk.SignMethod(parameters);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "docs.save", parameters);

            if (response["response"] != null)
                return VkDocumentAttachment.FromJson(response["response"].First);

            return null;
        }
    }
}
