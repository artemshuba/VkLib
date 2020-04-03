using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using VkLib.Extensions;

namespace VkLib.Core.Audio.Types
{
    public class VkPlaylistOriginal
    {
        /// <summary>
        /// Owner id
        /// </summary>
        [JsonProperty("owner_id")]
        public long OwnerId { get; set; }

        /// <summary>
        /// Playlist id
        /// </summary>
        [JsonProperty("playlist_id")]
        public long PlaylistId { get; set; }

        /// <summary>
        /// Access key
        /// </summary>
        [JsonProperty("access_key")]
        public string AccessKey { get; set; }
    }

    /// <summary>
    /// Differs from VkGenre by Name property
    /// </summary>
    public class VkPlaylistGenre
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
    }

    public class VkPlaylist
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Owner id
        /// </summary>
        public long OwnerId { get; set; }

        /// <summary>
        /// Type (0 - owned playlist, 1 - followed playlist)
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Genres
        /// </summary>
        public List<VkPlaylistGenre> Genres { get; set; }

        /// <summary>
        /// Artists
        /// </summary>
        public List<VkArtist> Artists { get; set; }

        /// <summary>
        /// Tracks count
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Is following
        /// </summary>
        public bool IsFollowing { get; set; }

        /// <summary>
        /// Number of followers
        /// </summary>
        public long Followers { get; set; }

        /// <summary>
        /// Number of plays
        /// </summary>
        public long Plays { get; set; }

        /// <summary>
        /// Creation time
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// Update time
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// Original
        /// </summary>
        public VkPlaylistOriginal Original { get; set; }

        /// <summary>
        /// Photo
        /// </summary>
        public VkThumb Photo { get; set; }

        /// <summary>
        /// Thumbs
        /// </summary>
        public List<VkThumb> Thumbs { get; set; }

        /// <summary>
        /// Access key
        /// </summary>
        public string AccessKey { get; set; }

        internal static VkPlaylist FromJson(JToken json)
        {
            if (json == null)
                throw new ArgumentException("Json can not be null.");

            var result = new VkPlaylist();

            result.Id = json["id"].Value<long>();
            result.OwnerId = json["owner_id"].Value<long>();
            result.Type = json["type"].Value<int>();
            result.Title = json["title"].Value<string>();
            result.Description = json["description"].Value<string>();

            result.Genres = json["genres"].ToObject<List<VkPlaylistGenre>>();
            result.Artists = json["main_artists"].ToObject<List<VkArtist>>();

            result.Count = json["count"].Value<int>();
            result.IsFollowing = json["is_following"].Value<bool>();

            result.Followers = json["followers"].Value<long>();
            result.Plays = json["plays"].Value<long>();

            result.CreateTime = DateTimeExtensions.UnixTimeStampToDateTime(json["create_time"].Value<long>());
            result.UpdateTime = DateTimeExtensions.UnixTimeStampToDateTime(json["update_time"].Value<long>());

            if (json["original"] != null)
                result.Original = json["original"].ToObject<VkPlaylistOriginal>();

            if (json["photo"] != null)
                result.Photo = json["photo"].ToObject<VkThumb>();

            if (json["thumbs"] != null)
                result.Thumbs = json["thumbs"].ToObject<List<VkThumb>>();

            if (json["access_key"] != null)
                result.AccessKey = json["access_key"].Value<string>();

            return result;
        }
    }
}