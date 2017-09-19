using System;
using Newtonsoft.Json.Linq;

namespace VkLib.Core.Documents
{
    /// <summary>
    /// Response from upload document requests
    /// </summary>
    public class VkUploadDocumentResponse
    {
        /// <summary>
        /// Server
        /// </summary>
        public string File { get; set; }

        public static VkUploadDocumentResponse FromJson(JObject json)
        {
            if (json == null)
                throw new ArgumentNullException(nameof(json));

            var result = new VkUploadDocumentResponse();
            result.File = (string)json["file"];

            return result;
        }
    }
}
