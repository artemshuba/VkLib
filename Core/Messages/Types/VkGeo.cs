using System;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json.Linq;
using VkLib.Core.Attachments;
using VkLib.Extensions;

namespace VkLib.Core.Messages
{
    public class VkGeo
    {
        public string Type { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public VkPlace Place { get; set; }

        internal static VkGeo FromJson(JToken json)
        {
            if (json == null)
                throw new Exception("Json can't be null");

            var result = new VkGeo();
            result.Type = (string)json["type"];

            var coordinatesString = (string)json["coordinates"];
            if (!string.IsNullOrEmpty(coordinatesString))
            {
                var coordinates = coordinatesString.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                if (coordinates.Length == 2)
                {

                    double lat;
                    double.TryParse(coordinates[0], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out lat);

                    double lon;
                    double.TryParse(coordinates[1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out lon);

                    result.Longitude = lon;
                    result.Latitude = lat;
                }
            }

            if (json["place"] != null)
                result.Place = VkPlace.FromJson(json["place"]);

            return result;
        }
    }
}
