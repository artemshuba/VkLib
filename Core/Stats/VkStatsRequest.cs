using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VkLib.Core.Stats
{
    public class VkStatsRequest
    {
        private readonly Vk _vkontakte;

        internal VkStatsRequest(Vk vkontakte)
        {
            _vkontakte = vkontakte;
        }

        /// <summary>
        /// Tracks user in stats
        /// </summary>
        /// <returns></returns>
        public async Task<bool> TrackVisitor()
        {
            if (_vkontakte.AccessToken == null || string.IsNullOrEmpty(_vkontakte.AccessToken.Token) || _vkontakte.AccessToken.HasExpired)
                throw new Exception("Access token is not valid.");

            var parameters = new Dictionary<string, string>();
            _vkontakte.SignMethod(parameters);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "stats.trackVisitor", parameters);

            if (response?["response"] != null)
                return true;

            return false;
        }
    }
}
