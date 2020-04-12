using System.Collections.Generic;
using VkLib.Core;
using VkLib.Core.Account;
using VkLib.Core.Audio;
using VkLib.Core.Auth;
using VkLib.Core.Documents;
using VkLib.Core.Execute;
using VkLib.Core.Favorites;
using VkLib.Core.Friends;
using VkLib.Core.Groups;
using VkLib.Core.Messages;
using VkLib.Core.News;
using VkLib.Core.Photos;
using VkLib.Core.Stats;
using VkLib.Core.Status;
using VkLib.Core.Storage;
using VkLib.Core.Store;
using VkLib.Core.Subscriptions;
using VkLib.Core.Users;
using VkLib.Core.Video;
using VkLib.Core.Wall;

namespace VkLib
{
    /// <summary>
    /// Core service for calling vk methods
    /// </summary>
    public class Vk
    {
        /// <summary>
        /// Application id
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// Client secret
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// User agent
        /// </summary>
        public string UserAgent
        {
            get { return VkRequest.UserAgent; }
            set { VkRequest.UserAgent = value; }
        }

        /// <summary>
        /// Api version
        /// </summary>
        public string ApiVersion { get; set; }

        /// <summary>
        /// Used in method execute.getBaseData to mark access token as "true" token to get access to the music
        /// </summary>
        public Dictionary<string, string> LoginParams { get; set; }

        /// <summary>
        /// Use HTTPS
        /// </summary>
        public bool UseHttps { get; set; }

        /// <summary>
        /// Language
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Access token
        /// </summary>
        public VkAccessToken AccessToken { get; set; }

        /// <summary>
        /// Audio
        /// </summary>
        public VkAudioRequest Audio => new VkAudioRequest(this);

        /// <summary>
        /// Users
        /// </summary>
        public VkUsersRequest Users => new VkUsersRequest(this);

        /// <summary>
        /// Friends
        /// </summary>
        public VkFriendsRequest Friends => new VkFriendsRequest(this);

        /// <summary>
        /// Groups
        /// </summary>
        public VkGroupsRequest Groups => new VkGroupsRequest(this);

        /// <summary>
        /// News
        /// </summary>
        public VkNewsRequest News => new VkNewsRequest(this);

        /// <summary>
        /// Wall
        /// </summary>
        public VkWallRequest Wall => new VkWallRequest(this);

        /// <summary>
        /// Favorites
        /// </summary>
        public VkFavoritesRequest Favorites => new VkFavoritesRequest(this);

        /// <summary>
        /// Status
        /// </summary>
        public VkStatusRequest Status => new VkStatusRequest(this);

        /// <summary>
        /// Video
        /// </summary>
        public VkVideoRequest Video => new VkVideoRequest(this);

        /// <summary>
        /// Messages
        /// </summary>
        public VkMessagesRequest Messages => new VkMessagesRequest(this);

        /// <summary>
        /// Photos
        /// </summary>
        public VkPhotosRequest Photos => new VkPhotosRequest(this);

        /// <summary>
        /// Long poll service
        /// </summary>
        public VkLongPollService LongPollService => new VkLongPollService(this);

        /// <summary>
        /// Account
        /// </summary>
        public VkAccountRequest Account => new VkAccountRequest(this);

        /// <summary>
        /// Subscriptions
        /// </summary>
        public VkSubscriptionsRequest Subscriptions => new VkSubscriptionsRequest(this);

        /// <summary>
        /// OAuth
        /// </summary>
        public VkOAuthRequest OAuth => new VkOAuthRequest(this);


        /// <summary>
        /// Direct Auth by login and password
        /// </summary>
        public VkDirectAuthRequest Auth => new VkDirectAuthRequest(this);

        /// <summary>
        /// Statistics
        /// </summary>
        public VkStatsRequest Stats => new VkStatsRequest(this);


        /// <summary>
        /// Storage
        /// </summary>
        public VkStorageRequest Storage => new VkStorageRequest(this);

        /// <summary>
        /// Execute
        /// </summary>
        public VkExecuteRequest Execute => new VkExecuteRequest(this);

        /// <summary>
        /// Documents
        /// </summary>
        public VkDocumentsRequest Documents => new VkDocumentsRequest(this);

        /// <summary>
        /// Store
        /// </summary>
        public VkStoreRequest Store => new VkStoreRequest(this);

        public Vk(string appId, string clientSecret, string apiVersion, string userAgent = null)
        {
            AccessToken = new VkAccessToken();
            ApiVersion = apiVersion;

            AppId = appId;
            ClientSecret = clientSecret;
            UserAgent = userAgent;
        }

        internal void SignMethod(Dictionary<string, string> parameters, string apiVersion = null)
        {
            if (parameters == null)
                parameters = new Dictionary<string, string>();

            parameters["access_token"] = AccessToken.Token;

            if (!string.IsNullOrEmpty(apiVersion))
                parameters["v"] = apiVersion;
            else if (!string.IsNullOrEmpty(ApiVersion))
                parameters["v"] = ApiVersion;

            if (UseHttps)
                parameters["https"] = "1";

            if (Language != null)
                parameters["lang"] = Language;
        }
    }
}