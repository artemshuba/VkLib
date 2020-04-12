using Newtonsoft.Json;

namespace VkLib.Core.Audio
{
    public class VkThumb
    {
        [JsonProperty("photo_34")]
        public string Photo34 { get; set; }

        [JsonProperty("photo_68")]
        public string Photo68 { get; set; }

        [JsonProperty("photo_135")]
        public string Photo135 { get; set; }

        [JsonProperty("photo_270")]
        public string Photo270 { get; set; }

        [JsonProperty("photo_300")]
        public string Photo300 { get; set; }

        [JsonProperty("photo_600")]
        public string Photo600 { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }
    }
}
