using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace VkLib.Core.Store
{
    public class VkStickerPackProduct
    {
        public string BaseUrl { get; set; }

        public List<int> StickerIds { get; set; }

        public static VkStickerPackProduct FromJson(JToken json)
        {
            if (json == null)
                throw new ArgumentNullException("json");

            var result = new VkStickerPackProduct();
            if (json["base_url"] != null)
                result.BaseUrl = (string)json["base_url"];

            if (json["sticker_ids"] != null)
            {
                result.StickerIds = json["sticker_ids"].Select(id => (int)id).ToList();
            }

            return result;
        }
    }
}