using System;
using Newtonsoft.Json.Linq;

namespace VkLib.Core.Audio
{
    /// <summary>
    /// Response from upload photo requests
    /// </summary>
    public class VkUploadAudioResponse
    {
        /// <summary>
        /// Server
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// Audio string. Should be passed unmodified.
        /// </summary>
        public string Audio { get; set; }

        /// <summary>
        /// Hash
        /// </summary>
        public string Hash { get; set; }

        public static VkUploadAudioResponse FromJson(JObject json)
        {
            if (json == null)
                throw new ArgumentNullException("json");

            var result = new VkUploadAudioResponse();
            result.Server = (string)json["server"];
            result.Audio = (string)json["audio"];
            result.Hash = (string)json["hash"];

            return result;
        }
    }
}
