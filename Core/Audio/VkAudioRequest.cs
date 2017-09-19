using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using VkLib.Core.Audio.Types;

namespace VkLib.Core.Audio
{
    /// <summary>
    /// Audios sort type
    /// </summary>
    public enum VkAudioSortType
    {
        /// <summary>
        /// Sort by date added
        /// </summary>
        DateAdded,
        /// <summary>
        /// Sort by duration
        /// </summary>
        Duration,
        /// <summary>
        /// Sort by popularity
        /// </summary>
        Popularity
    }

    /// <summary>
    /// Audio request
    /// </summary>
    public class VkAudioRequest
    {
        private const int MAX_AUDIO_COUNT = 300;

        private readonly Vk _vkontakte;

        internal VkAudioRequest(Vk vkontakte)
        {
            _vkontakte = vkontakte;
        }

        /// <summary>
        /// Get all audios of current user.
        /// See also <see cref="http://vk.com/dev/audio.get"/>
        /// </summary>
        /// <returns></returns>
        public async Task<VkItemsResponse<VkAudio>> Get()
        {
            return await Get(_vkontakte.AccessToken.UserId);
        }

        /// <summary>
        /// Get audios of user or society.
        /// See also <see cref="http://vk.com/dev/audio.get"/>
        /// </summary>
        /// <param name="ownerId">Owner id. For society must be negative.</param>
        /// <param name="albumId">Album id</param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public async Task<VkItemsResponse<VkAudio>> Get(long ownerId, long albumId = 0, int count = 0, int offset = 0, string accessKey = null)
        {
            var parameters = new Dictionary<string, string>();

            if (ownerId != 0)
                parameters["owner_id"] = ownerId.ToString(CultureInfo.InvariantCulture);

            if (albumId != 0)
                parameters["album_id"] = albumId.ToString(CultureInfo.InvariantCulture);

            if (count > 0)
                parameters["count"] = count.ToString(CultureInfo.InvariantCulture);

            if (offset > 0)
                parameters["offset"] = offset.ToString(CultureInfo.InvariantCulture);
            
            if (!string.IsNullOrEmpty(accessKey))
                parameters["access_key"] = accessKey;

            _vkontakte.SignMethod(parameters);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "audio.get", parameters);

            if (response.SelectToken("response.items") != null)
            {
                return new VkItemsResponse<VkAudio>(response["response"]["items"].Select(VkAudio.FromJson).ToList(), (int)response["response"]["count"]);
            }

            return VkItemsResponse<VkAudio>.Empty;
        }

        /// <summary>
        /// Get albums of user or society.
        /// See also <see cref="http://vk.com/dev/audio.getAlbums"/>
        /// </summary>
        /// <param name="ownerId">Owner id. For society must be negative.</param>
        /// <param name="count">Count</param>
        /// <param name="offset">Offset</param>
        /// <returns></returns>
        public async Task<VkItemsResponse<VkAudioAlbum>> GetAlbums(long ownerId, int count = 0, int offset = 0)
        {
            var parameters = new Dictionary<string, string>();

            if (ownerId != 0)
                parameters["owner_id"] = ownerId.ToString(CultureInfo.InvariantCulture);

            if (count > 0)
                parameters["count"] = count.ToString(CultureInfo.InvariantCulture);

            if (offset > 0)
                parameters["offset"] = offset.ToString(CultureInfo.InvariantCulture);

            _vkontakte.SignMethod(parameters);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "audio.getAlbums", parameters);

            if (response.SelectToken("response.items") != null)
            {
                return new VkItemsResponse<VkAudioAlbum>(response["response"]["items"].Select(VkAudioAlbum.FromJson).ToList(), (int)response["response"]["count"]);
            }

            return VkItemsResponse<VkAudioAlbum>.Empty;
        }

        public async Task<VkItemsResponse<VkPlaylist>> GetPlaylists(long ownerId, int count = 0, int offset = 0)
        {
            var parameters = new Dictionary<string, string>();

            parameters["owner_id"] = ownerId.ToString(CultureInfo.InvariantCulture);

            if (count > 0)
                parameters["count"] = count.ToString(CultureInfo.InvariantCulture);

            if (offset > 0)
                parameters["offset"] = offset.ToString(CultureInfo.InvariantCulture);

            _vkontakte.SignMethod(parameters);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "audio.getPlaylists", parameters);

            if (response.SelectToken("response.items") != null)
            {
                return new VkItemsResponse<VkPlaylist>(response["response"]["items"].Select(VkPlaylist.FromJson).ToList(), (int)response["response"]["count"]);
            }

            return VkItemsResponse<VkPlaylist>.Empty;
        }

        /// <summary>
        /// Search audios.
        /// See also <see cref="http://vk.com/dev/audio.search"/>
        /// </summary>
        /// <param name="query">Query</param>
        /// <param name="count">Count</param>
        /// <param name="offset">Offset</param>
        /// <param name="sort">Sort</param>
        /// <param name="withLyricsOnly">If true will show only audios with lyrics</param>
        /// <param name="autoFix">If true will fix incorrect queries</param>
        /// <param name="artistOnly">If true will search only by artist</param>
        /// <param name="ownOnly">If true will search only in audios of current user</param>
        /// <returns></returns>
        public async Task<VkItemsResponse<VkAudio>> Search(string query, int count = 0, int offset = 0, VkAudioSortType sort = VkAudioSortType.DateAdded, bool withLyricsOnly = false, bool autoFix = true,
            bool artistOnly = false, bool ownOnly = false)
        {
            if (count > MAX_AUDIO_COUNT)
                throw new ArgumentException("Maximum count is " + MAX_AUDIO_COUNT);

            if (query == null)
                throw new ArgumentException("Query must not be null.");

            var parameters = new Dictionary<string, string>();

            parameters["q"] = query;

            if (autoFix)
                parameters["auto_complete"] = "1";

            parameters["sort"] = ((int)sort).ToString(CultureInfo.InvariantCulture);

            if (withLyricsOnly)
                parameters["lyrics"] = "1";

            if (artistOnly)
                parameters["performer_only"] = "1";

            if (ownOnly)
                parameters["search_own"] = "1";

            if (count > 0)
                parameters["count"] = count.ToString(CultureInfo.InvariantCulture);
            else
                parameters["count"] = MAX_AUDIO_COUNT.ToString(CultureInfo.InvariantCulture);

            if (offset > 0)
                parameters["offset"] = offset.ToString(CultureInfo.InvariantCulture);

            _vkontakte.SignMethod(parameters);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "audio.search", parameters);

            if (response.SelectToken("response.items") != null)
            {
                return new VkItemsResponse<VkAudio>((from a in response["response"]["items"] where a.HasValues && !string.IsNullOrEmpty(a["url"].Value<string>()) select VkAudio.FromJson(a)).ToList(),
                    response["response"]["count"].Value<int>());
            }

            return VkItemsResponse<VkAudio>.Empty;
        }

        /// <summary>
        /// Add audio to current user or society.
        /// See also <see cref="http://vk.com/dev/audio.add"/>
        /// </summary>
        /// <param name="audioId">Audio id</param>
        /// <param name="ownerId">Owner id</param>
        /// <param name="groupId">Target society</param>
        /// <param name="captchaSid">Captcha sid</param>
        /// <param name="captchaKey">Captcha key</param>
        /// <returns>Id of new audio</returns>
        public async Task<long> Add(long audioId, long ownerId, long groupId = 0, string captchaSid = null, string captchaKey = null)
        {
            var parameters = new Dictionary<string, string>();

            parameters["audio_id"] = audioId.ToString();

            parameters["owner_id"] = ownerId.ToString();

            if (groupId != 0)
                parameters.Add("group_id", groupId.ToString());

            if (!string.IsNullOrEmpty(captchaSid))
                parameters.Add("captcha_sid", captchaSid);

            if (!string.IsNullOrEmpty(captchaKey))
                parameters.Add("captcha_key", captchaKey);

            _vkontakte.SignMethod(parameters);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "audio.add", parameters);

            if (response["response"] != null)
            {
                return response["response"].Value<long>();
            }

            return 0;
        }

        /// <summary>
        /// Remove audio from current user or society.
        /// See also <see cref="http://vk.com/dev/audio.delete"/>
        /// </summary>
        /// <param name="audioId">Audio id</param>
        /// <param name="ownerId">Owner id</param>
        /// <returns>True if success</returns>
        public async Task<bool> Delete(long audioId, long ownerId)
        {
            var parameters = new Dictionary<string, string>();

            parameters.Add("audio_id", audioId.ToString());

            parameters.Add("owner_id", ownerId.ToString());

            _vkontakte.SignMethod(parameters);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "audio.delete", parameters);

            if (response["response"]?.Value<long>() == 1)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Move audios to album.
        /// See also <see cref="http://vk.com/dev/audio.moveToAlbum"/>
        /// </summary>
        /// <param name="albumId">Album id</param>
        /// <param name="audioIds">List of audios ids</param>
        /// <param name="groupId">Source group id. If not specified, current user will be used.</param>
        /// <returns>True if success</returns>
        public async Task<bool> MoveToAlbum(long albumId, IList<long> audioIds, long groupId = 0)
        {
            var parameters = new Dictionary<string, string>();

            parameters.Add("album_id", albumId.ToString());

            parameters.Add("audio_ids", string.Join(",", audioIds));

            _vkontakte.SignMethod(parameters);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "audio.moveToAlbum", parameters);

            if (response["response"]?.Value<long>() == 1)
            {
                return true;
            }

            return false;
        }

        public async Task<List<VkAudio>> GetById(IList<string> ids)
        {
            var parameters = new Dictionary<string, string>();

            parameters.Add("audios", string.Join(",", ids));

            _vkontakte.SignMethod(parameters);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "audio.getById", parameters);

            if (response["response"]?.HasValues == true)
            {
                return (from a in response["response"] select VkAudio.FromJson(a)).ToList();
            }

            return null;
        }

        public async Task<string> GetLyrics(long lyricsId)
        {
            var parameters = new Dictionary<string, string>();

            parameters.Add("lyrics_id", lyricsId.ToString());

            _vkontakte.SignMethod(parameters);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "audio.getLyrics", parameters);

            if (response.SelectToken("response.text") != null)
            {
                var text = response.SelectToken("response.text").Value<string>();
                if (!string.IsNullOrEmpty(text))
                    return WebUtility.HtmlDecode(text);
            }

            return null;
        }

        public async Task<VkItemsResponse<VkAudio>> GetRecommendations(string targetAudio = null, int count = 0, int offset = 0, long userId = 0, bool shuffle = false)
        {
            var parameters = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(targetAudio))
                parameters.Add("target_audio", targetAudio);

            if (userId > 0)
                parameters.Add("user_id", userId.ToString(CultureInfo.InvariantCulture));

            if (count > 0)
                parameters.Add("count", count.ToString(CultureInfo.InvariantCulture));

            if (offset > 0)
                parameters.Add("offset", offset.ToString(CultureInfo.InvariantCulture));

            if (shuffle)
                parameters.Add("shuffle", "1");

            _vkontakte.SignMethod(parameters);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "audio.getRecommendations", parameters);

            var token = response.SelectToken("response.items");
            if (token != null && token.HasValues)
            {
                return new VkItemsResponse<VkAudio>((from a in token select VkAudio.FromJson(a)).ToList());
            }

            return VkItemsResponse<VkAudio>.Empty;
        }

        public async Task<VkItemsResponse<VkAudio>> GetPopular(bool onlyEng = false, int count = 0, int offset = 0, int genreId = 0)
        {
            var parameters = new Dictionary<string, string>();

            if (onlyEng)
                parameters.Add("only_eng", "1");

            if (genreId != 0)
                parameters.Add("genre_id", genreId.ToString(CultureInfo.InvariantCulture));

            if (count > 0)
                parameters.Add("count", count.ToString(CultureInfo.InvariantCulture));

            if (offset > 0)
                parameters.Add("offset", offset.ToString(CultureInfo.InvariantCulture));

            _vkontakte.SignMethod(parameters);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "audio.getPopular", parameters);

            if (response["response"]?.HasValues == true)
            {
                return new VkItemsResponse<VkAudio>((from a in response["response"] select VkAudio.FromJson(a)).ToList());
            }

            return VkItemsResponse<VkAudio>.Empty;
        }

        public async Task<long> Edit(long ownerId, long audioId, string artist = null, string title = null, string text = null, int genreId = 0, bool noSearch = false)
        {
            var parameters = new Dictionary<string, string>();

            const string method = "audio.edit";

            parameters["owner_id"] = ownerId.ToString();
            parameters["audio_id"] = audioId.ToString();

            if (!string.IsNullOrEmpty(artist))
                parameters["artist"] = artist;

            if (!string.IsNullOrEmpty(title))
                parameters["title"] = title;

            if (!string.IsNullOrEmpty(text))
                parameters["text"] = text;
            else
                parameters["text"] = string.Empty;

            if (noSearch)
                parameters.Add("no_search", "1");

            _vkontakte.SignMethod(parameters);

            var response = await VkRequest.PostAsync(VkConst.MethodBase + method, parameters);

            return response["response"]?.Value<long>() ?? 0;
        }

        public async Task<long> AddAlbum(string title, long groupId = 0)
        {
            var parameters = new Dictionary<string, string>();

            parameters.Add("title", title);

            if (groupId > 0)
                parameters.Add("group_id", groupId.ToString());

            _vkontakte.SignMethod(parameters);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "audio.addAlbum", parameters);

            if (response.SelectToken("response.album_id") != null)
            {
                return response["response"]["album_id"].Value<long>();
            }

            return 0;
        }

        public async Task<bool> DeleteAlbum(long albumId, long groupId = 0)
        {
            var parameters = new Dictionary<string, string>();

            parameters.Add("album_id", albumId.ToString());

            if (groupId > 0)
                parameters.Add("group_id", groupId.ToString());

            _vkontakte.SignMethod(parameters);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "audio.deleteAlbum", parameters);

            return response["response"]?.Value<int>() == 1;
        }

        public async Task<bool> EditAlbum(string albumId, string title, long groupId = 0)
        {
            var parameters = new Dictionary<string, string>();

            parameters.Add("album_id", albumId);
            parameters.Add("title", title);

            if (groupId > 0)
                parameters.Add("group_id", groupId.ToString());

            _vkontakte.SignMethod(parameters);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "audio.editAlbum", parameters);

            return response["response"]?.Value<int>() == 1;
        }

        public async Task<long> FollowPlaylist(long ownerId, long playlistId, string accessKey = null)
        {
            var parameters = new Dictionary<string, string>();

            parameters.Add("owner_id", ownerId.ToString());
            parameters.Add("playlist_id", playlistId.ToString());

            if (!string.IsNullOrEmpty(accessKey))
                parameters["access_key"] = accessKey;

            _vkontakte.SignMethod(parameters);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "audio.followPlaylist", parameters);

            return response.SelectToken("response.playlist_id")?.Value<long>() ?? 0;
        }

        public async Task<bool> DeletePlaylist(long playlistId, long ownerId)
        {
            var parameters = new Dictionary<string, string>();

            parameters.Add("playlist_id", playlistId.ToString());

            parameters.Add("owner_id", ownerId.ToString());

            _vkontakte.SignMethod(parameters);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "audio.deletePlaylist", parameters);

            return response["response"]?.Value<int>() == 1;
        }

        public async Task<bool> Reorder(long audioId, long after, long before, long ownerId = 0)
        {
            var parameters = new Dictionary<string, string>();

            parameters.Add("audio_id", audioId.ToString());

            if (before != 0)
                parameters.Add("before", before.ToString());

            if (after != 0)
                parameters.Add("after", after.ToString());

            if (ownerId != 0)
                parameters.Add("owner_id", ownerId.ToString());

            _vkontakte.SignMethod(parameters);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "audio.reorder", parameters);

            return response["response"]?.Value<int>() == 1;
        }

        public async Task<List<long>> SetBroadcast(long audioId, long ownerId, IList<long> targetIds = null)
        {
            var parameters = new Dictionary<string, string>();

            if (audioId != 0 && ownerId != 0)
                parameters.Add("audio", $"{ownerId}_{audioId}");

            if (targetIds != null)
                parameters.Add("target_ids", string.Join(",", targetIds));

            _vkontakte.SignMethod(parameters);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "audio.setBroadcast", parameters);

            if (response["response"]?.HasValues == true)
            {
                return response["response"].Values<long>().ToList<long>();
            }

            return null;
        }

        /// <summary>
        /// Get list of genres
        /// </summary>
        /// <returns></returns>
        public List<VkGenre> GetGenres()
        {
            //http://vk.com/dev/audio_genres

            var genres = new List<VkGenre>();
            genres.Add(new VkGenre() { Id = 1, Title = "Rock" });
            genres.Add(new VkGenre() { Id = 2, Title = "Pop" });
            genres.Add(new VkGenre() { Id = 3, Title = "Rap & Hip-Hop" });
            genres.Add(new VkGenre() { Id = 4, Title = "Easy Listening" });
            genres.Add(new VkGenre() { Id = 5, Title = "Dance & House" });
            genres.Add(new VkGenre() { Id = 6, Title = "Instrumental" });
            genres.Add(new VkGenre() { Id = 7, Title = "Metal" });
            genres.Add(new VkGenre() { Id = 21, Title = "Alternative" });
            genres.Add(new VkGenre() { Id = 8, Title = "Dubstep" });
            genres.Add(new VkGenre() { Id = 1001, Title = "Jazz & Blues" });
            genres.Add(new VkGenre() { Id = 10, Title = "Drum & Bass" });
            genres.Add(new VkGenre() { Id = 11, Title = "Trance" });
            genres.Add(new VkGenre() { Id = 12, Title = "Chanson" });
            genres.Add(new VkGenre() { Id = 13, Title = "Ethnic" });
            genres.Add(new VkGenre() { Id = 14, Title = "Acoustic & Vocal" });
            genres.Add(new VkGenre() { Id = 15, Title = "Reggae" });
            genres.Add(new VkGenre() { Id = 16, Title = "Classical" });
            genres.Add(new VkGenre() { Id = 17, Title = "Indie Pop" });
            genres.Add(new VkGenre() { Id = 19, Title = "Speech" });
            genres.Add(new VkGenre() { Id = 22, Title = "Electropop & Disco" });
            genres.Add(new VkGenre() { Id = 18, Title = "Other" });

            return genres;
        }

        public async Task<string> GetUploadServer()
        {
            var parameters = new Dictionary<string, string>();

            _vkontakte.SignMethod(parameters);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "audio.getUploadServer", parameters);

            return response["response"]["upload_url"]?.Value<string>();
        }

        public async Task<VkCatalogBlock> GetCatalogBlockById(VkCatalogBlockType block)
        {
            var parameters = new Dictionary<string, string>();

            parameters.Add("block_id", ((int)block).ToString());

            _vkontakte.SignMethod(parameters);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "audio.getCatalogBlockById", parameters);

            if (response.SelectToken("response.block") != null)
            {
                return VkCatalogBlock.FromJson(response.SelectToken("response.block"));
            }

            return null;
        }

        public async Task<List<VkCatalogBlock>> GetCatalog()
        {
            var parameters = new Dictionary<string, string>();

            _vkontakte.SignMethod(parameters);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "audio.getCatalog", parameters);

            if (response.SelectToken("response.items") != null)
            {
                return response.SelectToken("response.items").Select(VkCatalogBlock.FromJson).ToList();
            }

            return null;
        }
    }
}