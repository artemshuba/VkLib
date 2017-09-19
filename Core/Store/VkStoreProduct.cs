using System;
using Newtonsoft.Json.Linq;
using VkLib.Extensions;

namespace VkLib.Core.Store
{
    public class VkStoreProduct
    {
        public long Id { get; set; }

        public string Type { get; set; }

        public bool IsPurchased { get; set; }

        public bool IsActive { get; set; }

        public DateTime PurchaseDate { get; set; }

        public string Title { get; set; }

        public string BaseUrl { get; set; }

        public VkStickerPackProduct Stickers { get; set; }

        public static VkStoreProduct FromJson(JToken json)
        {
            if (json == null)
                throw new ArgumentNullException("json");

            var result = new VkStoreProduct();
            result.Id = (long)json["id"];

            if (json["type"] != null)
                result.Type = (string)json["type"];

            if (json["purchased"] != null)
                result.IsPurchased = (int)json["purchased"] == 1;

            if (json["active"] != null)
                result.IsActive = (int)json["active"] == 1;

            if (json["purchase_date"] != null)
                result.PurchaseDate = DateTimeExtensions.UnixTimeStampToDateTime((double)json["purchase_date"]);

            if (json["title"] != null)
                result.Title = (string)json["title"];

            if (json["base_url"] != null)
                result.BaseUrl = (string)json["base_url"];

            if (json["stickers"] != null)
                result.Stickers = VkStickerPackProduct.FromJson(json["stickers"]);

            return result;
        }
    }
}