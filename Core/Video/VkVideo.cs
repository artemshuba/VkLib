using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json.Linq;

namespace VkLib.Core.Video
{
    public class VkVideo
    {
        public long Id { get; set; }

        public long OwnerId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public TimeSpan Duration { get; set; }

        /// <summary>
        /// 130x98
        /// </summary>
        public string Photo130 { get; set; }

        /// <summary>
        /// 320x240
        /// </summary>
        public string Photo320 { get; set; }

        /// <summary>
        /// 640x480
        /// </summary>
        public string Photo640 { get; set; }

        public string PhotoMax
        {
            get { return Photo640 ?? Photo320 ?? Photo130; }
        }

        public DateTime Date { get; set; }

        public string Player { get; set; }

        public Dictionary<string, string> Files { get; set; }

        internal static VkVideo FromJson(JToken json)
        {
            if (json == null)
                throw new ArgumentException("Json can not be null.");

            var result = new VkVideo();

            if (json["id"] != null)
                result.Id = json["id"].Value<long>();

            result.OwnerId = json["owner_id"].Value<long>();
            result.Duration = TimeSpan.FromSeconds(json["duration"].Value<double>());
            result.Title = WebUtility.HtmlDecode(json["title"].Value<string>());
            result.Description = WebUtility.HtmlDecode(json["description"].Value<string>());

            if (json["photo_130"] != null)
                result.Photo130 = json["photo_130"].Value<string>();

            if (json["photo_320"] != null)
                result.Photo130 = json["photo_320"].Value<string>();

            if (json["photo_640"] != null)
                result.Photo130 = json["photo_640"].Value<string>();

            if (json["files"] != null)
            {
                result.Files = new Dictionary<string, string>();

                foreach (JProperty child in json["files"].Children())
                {
                    if (!child.HasValues)
                        continue;

                    result.Files.Add(child.Name, child.Value.Value<string>());
                }
            }

            if (json["player"] != null)
                result.Player = json["player"].Value<string>();

            return result;
        }
    }
}
