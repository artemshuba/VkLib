using System.Collections.Generic;
using Newtonsoft.Json;

namespace VkLib.Core.Push
{

    public class VkPushMessageSettings
    {
        /// <summary>
        /// Is enabled
        /// </summary>
        public bool? IsEnabled { get; set; }

        /// <summary>
        /// Disable sound
        /// </summary>
        public bool NoSound { get; set; }

        /// <summary>
        /// Disable text
        /// </summary>
        public bool NoText { get; set; }
    }

    public class VkPushSettings
    {
        public VkPushMessageSettings Messages { get; set; }

        public VkPushMessageSettings Chat { get; set; }

        public VkPushSettings()
        {

        }

        public string ToJson()
        {
            var parameters = new Dictionary<string, object>();

            if (Messages != null)
            {
                if (Messages.IsEnabled != null)
                    parameters.Add("msg", Messages.IsEnabled.Value ? "on" : "off");
                else
                {
                    var msg = new List<string>();
                    if (Messages.NoSound)
                        msg.Add("no_sound");
                    if (Messages.NoText)
                        msg.Add("no_text");
                    parameters.Add("msg", msg);
                }
            }

            if (Chat != null)
            {
                if (Chat.IsEnabled != null)
                    parameters.Add("chat", Chat.IsEnabled.Value ? "on" : "off");
                else
                {
                    var chat = new List<string>();
                    if (Chat.NoSound)
                        chat.Add("no_sound");
                    if (Chat.NoText)
                        chat.Add("no_text");
                    parameters.Add("msg", chat);
                }
            }

            return JsonConvert.SerializeObject(parameters);
        }
    }
}
