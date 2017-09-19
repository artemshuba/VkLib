namespace VkLib.Core.Store
{
    public class VkStickerProduct
    {
        public int Id { get; set; }

        public string BaseUrl { get; set; }

        public string GetPreviewUrl(int size)
        {
            return BaseUrl + Id + "/" + size + ".png";
        }
    }
}
