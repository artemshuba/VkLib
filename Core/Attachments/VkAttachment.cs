using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using VkLib.Core.Messages;

namespace VkLib.Core.Attachments
{
    public class VkAttachment
    {
        public long Id { get; set; }

        public long OwnerId { get; set; }

        public virtual string Type { get; set; }

        public override string ToString()
        {
            return $"{Type}{OwnerId}_{Id}";
        }

        public static List<VkAttachment> FromJson(JToken json)
        {
            var result = new List<VkAttachment>();

            foreach (var a in json)
            {
                switch (a["type"].Value<string>())
                {
                    case "audio":
                        result.Add(VkAudioAttachment.FromJson(a["audio"]));
                        break;

                    case "photo":
                        result.Add(VkPhotoAttachment.FromJson(a["photo"]));
                        break;

                    case "sticker":
                        result.Add(VkStickerAttachment.FromJson(a["sticker"]));
                        break;

                    case "gift":
                        result.Add(VkGiftAttachment.FromJson(a["gift"]));
                        break;

                    case "link":
                        result.Add(VkLinkAttachment.FromJson(a["link"]));
                        break;

                    case "doc":
                        result.Add(VkDocumentAttachment.FromJson(a["doc"]));
                        break;

                    case "video":
                        result.Add(VkVideoAttachment.FromJson(a["video"]));
                        break;

                    case "wall":
                        result.Add(VkWallPostAttachment.FromJson(a["wall"]));
                        break;
                }
            }

            return result;
        }
    }
}
