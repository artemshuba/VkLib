using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using VkLib.Core.Groups;
using VkLib.Core.Users;

namespace VkLib.Core.Audio.Types
{
    /// <summary>
    /// Catalog block
    /// </summary>
    public enum VkCatalogBlockType
    {
        /// <summary>
        /// Special for you block
        /// </summary>
        SpecialForYou = 1,
        /// <summary>
        /// Friends block
        /// </summary>
        FriendsMusic = 3,
        /// <summary>
        /// Societies block
        /// </summary>
        SocietiesMusic = 4,
        /// <summary>
        /// Recent listened tracks block
        /// </summary>
        RecentMusic = 6,
        /// <summary>
        /// Popular music block
        /// </summary>
        Popular = 8,
        /// <summary>
        /// New music block
        /// </summary>
        New = 14,
        /// <summary>
        /// New albums block
        /// </summary>
        NewAlbums = 20,
        /// <summary>
        /// New artists
        /// </summary>
        NewArtists = 21,
        /// <summary>
        /// You may like block
        /// </summary>
        YouMayLike = 22
    }

    /// <summary>
    /// Catalog block
    /// </summary>
    public class VkCatalogBlock
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Subtitle
        /// </summary>
        public string Subtitle { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Items count
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Source
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Audios
        /// </summary>
        public List<VkAudio> Audios { get; set; }

        /// <summary>
        /// Thumbs
        /// </summary>
        public List<VkThumb> Thumbs { get; set; }

        /// <summary>
        /// Owners
        /// </summary>
        public List<VkProfileBase> Owners { get; set; }

        /// <summary>
        /// Playlists
        /// </summary>
        public List<VkPlaylist> Playlists { get; set; }

        /// <summary>
        /// Extended playlist
        /// </summary>
        public List<VkExtendedPlaylist> ExtendedPlaylists { get; set; }

        internal static VkCatalogBlock FromJson(JToken json)
        {
            if (json == null)
                throw new ArgumentException("Json can not be null.");

            var result = new VkCatalogBlock();

            result.Id = json["id"].Value<int>();
            result.Title = json["title"].Value<string>();
            result.Subtitle = json["subtitle"].Value<string>();

            result.Type = json["type"].Value<string>();
            result.Count = json["count"].Value<int>();

            result.Source = json["source"].Value<string>();

            if (json["audios"] != null)
                result.Audios = json["audios"].Select(VkAudio.FromJson).ToList();

            if (json["thumbs"] != null)
                result.Thumbs = json["thumbs"].ToObject<List<VkThumb>>();

            if (json["owners"] != null)
                result.Owners = json["owners"].Select(x =>
                {
                    if (x["type"].Value<string>() != "profile")
                        return (VkProfileBase)VkGroup.FromJson(x);
                    else
                        return (VkProfileBase)VkProfile.FromJson(x);
                }).ToList();

            if (json["playlists"] != null)
                result.Playlists = json["playlists"].Select(VkPlaylist.FromJson).ToList();

            if (json["extended_playlists"] != null)
                result.ExtendedPlaylists = json["extended_playlists"].Select(VkExtendedPlaylist.FromJson).ToList();

            return result;
        }
    }
}
