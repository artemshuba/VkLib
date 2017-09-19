using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VkLib.Core.Push;
using Newtonsoft.Json.Linq;

namespace VkLib.Core.Account
{
    public class VkAccountRequest
    {
        private readonly Vk _vkontakte;

        public VkAccountRequest(Vk vkontakte)
        {
            _vkontakte = vkontakte;
        }

        public async Task<bool> RegisterDevice(string token, string deviceModel = null, string deviceId = null, string systemVersion = null, VkPushSettings settings = null)
        {
            var parameters = new Dictionary<string, string>();

            parameters["token"] = token;

            if (!string.IsNullOrEmpty(deviceModel))
                parameters["device_model"] = deviceModel;

            if (!string.IsNullOrEmpty(deviceId))
                parameters["device_id"] = deviceId;

            if (!string.IsNullOrEmpty(systemVersion))
                parameters["system_version"] = systemVersion;

            if (settings != null)
                parameters["settings"] = settings.ToJson();

            _vkontakte.SignMethod(parameters);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "account.registerDevice", parameters);

            return response["response"]?.Value<int>() == 1;
        }

        public async Task<bool> UnregisterDevice(string deviceId = null)
        {
            var parameters = new Dictionary<string, string>();

            parameters["device_id"] = deviceId;

            _vkontakte.SignMethod(parameters);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "account.unregisterDevice", parameters);

            return response["response"]?.Value<int>() == 1;
        }


        public async Task<bool> SetOnline(bool voip = false)
        {
            var parametres = new Dictionary<string, string>();

            if (voip)
                parametres["voip"] = "1";

            _vkontakte.SignMethod(parametres);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "account.setOnline", parametres);

            return response["response"]?.Value<int>() == 1;
        }

        public async Task<bool> SetOffline()
        {
            var parametres = new Dictionary<string, string>();

            _vkontakte.SignMethod(parametres);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "account.setOffline", parametres);

            return response["response"]?.Value<int>() == 1;
        }
    }
}
