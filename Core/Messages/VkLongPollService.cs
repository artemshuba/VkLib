using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace VkLib.Core.Messages
{
    public class VkLongPollService
    {
        private readonly Vk _vkontakte;
        private bool _stop;

        public VkLongPollService(Vk vkontakte)
        {
            _vkontakte = vkontakte;
        }

        public async Task Start(CancellationToken cancellationToken, Action<List<VkLongPollMessage>> onMessage = null)
        {
            var parameters = new Dictionary<string, string>();
            _vkontakte.SignMethod(parameters);

            var response = await VkRequest.GetAsync(VkConst.MethodBase + "messages.getLongPollServer", parameters);

            if (response["response"] != null)
            {
                var key = (string)response["response"]["key"];
                var server = (string)response["response"]["server"];
                var ts = (string)response["response"]["ts"];

                Debug.WriteLine("VkLib Long poll service started: " + response);

                await Connect(key, server, ts, cancellationToken, onMessage);
            }

        }

        public void Stop()
        {
            _stop = true;
        }

        private async Task Connect(string key, string server, string ts, CancellationToken cancellationToken, Action<List<VkLongPollMessage>> onMessage = null)
        {
            var parametres = new Dictionary<string, string>();

            parametres.Add("act", "a_check");
            parametres.Add("key", key);
            parametres.Add("ts", ts);
            parametres.Add("wait", "25");
            parametres.Add("mode", "2");

            var response = await VkRequest.GetAsync("https://" + server, parametres);

            if (cancellationToken.IsCancellationRequested)
                return;

            if (response != null)
            {
                Debug.WriteLine("Long poll service response: " + response);

                ts = (string)response["ts"];

                var result = new List<VkLongPollMessage>();

                foreach (JArray update in response["updates"])
                {
                    var m = VkLongPollMessage.FromJson(update);
                    if (m != null)
                        result.Add(m);
                }

                onMessage?.Invoke(result);

                if (!_stop)
                    await Connect(key, server, ts, cancellationToken, onMessage);
            }
        }
    }
}
