using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace VkLib.Core.Storage
{
    public class VkStorageRequest
    {
        private readonly Vk _vkontakte;

        internal VkStorageRequest(Vk vkontakte)
        {
            _vkontakte = vkontakte;
        }

        public async Task<bool> Set(string key, string value)
        {
            var parameters = new Dictionary<string, string>();

            parameters.Add("key", key);

            if (!string.IsNullOrEmpty(value))
                parameters.Add("value", value);

            parameters.Add("access_token", _vkontakte.AccessToken.Token);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "storage.set", parameters);

            return response["response"]?.Value<int>() == 1;
        }
        public async Task<string> Get(string key)
        {
            var parameters = new Dictionary<string, string>();

            parameters.Add("key", key);

            parameters.Add("access_token", _vkontakte.AccessToken.Token);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "storage.get", parameters);

            return response["response"]?.Value<string>();
        }
    }
}
