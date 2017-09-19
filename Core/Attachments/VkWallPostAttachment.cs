using System;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using VkLib.Core.Wall;
using VkLib.Extensions;

namespace VkLib.Core.Attachments
{
    public class VkWallPostAttachment : VkAttachment
    {
        public long FromId { get; set; }

        public long ToId { get; set; }

        public DateTime Date { get; set; }

        public string PostType { get; set; }

        public string Text { get; set; }

        public List<VkWallEntry> CopyHistory { get; set; }

        public bool CanDelete { get; set; }

        //TODO some other fields

        /// <summary>
        /// Type
        /// </summary>
        public override string Type => "wall";

        public string GetLink()
        {
            return $"https://vk.com/wall{FromId}_{Id}";
        }

        public static new VkWallPostAttachment FromJson(JToken json)
        {
            if (json == null)
                throw new ArgumentException("Json can not be null.");

            var result = new VkWallPostAttachment();

            result.Id = (long)json["id"];

            if (json["owner_id"] != null)
                result.OwnerId = (long)json["owner_id"];

            if (json["from_id"] != null)
                result.FromId = (long)json["from_id"];

            if (json["to_id"] != null)
                result.ToId = (long)json["to_id"];

            if (json["date"] != null)
                result.Date = DateTimeExtensions.UnixTimeStampToDateTime(json["date"].Value<long>());

            if (json["post_type"] != null)
                result.PostType = (string)json["post_type"];

            if (json["text"] != null)
                result.Text = (string)json["text"];

            if (json["copy_history"] != null)
            {
                result.CopyHistory = new List<VkWallEntry>();

                foreach (var p in json["copy_history"])
                {
                    try
                    {
                        var post = VkWallEntry.FromJson(p);
                        if (post != null)
                            result.CopyHistory.Add(post);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                }
            }


            if (json["can_delete"] != null)
                result.CanDelete = (int)json["can_delete"] == 1;

            return result;
        }
    }
}
