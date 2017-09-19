using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json.Linq;
using VkLib.Core.Attachments;
using VkLib.Extensions;

namespace VkLib.Core.Messages
{
    /// <summary>
    /// Тип события, возвращаемого LongPoll-сервером
    /// </summary>
    public enum VkLongPollMessageType
    {
        /// <summary>
        /// Неизвестный тип
        /// </summary>
        Unknown = -1,
        /// <summary>
        /// Удаление сообщения
        /// </summary>
        MessageDelete = 0,
        /// <summary>
        /// Замена флагов сообщения
        /// </summary>
        MessageFlagUpdate = 1,
        /// <summary>
        /// Установка флагов сообщения
        /// </summary>
        MessageFlagSet = 2,
        /// <summary>
        /// Сброс флагов сообщения
        /// </summary>
        MessageFlagReset = 3,
        /// <summary>
        /// Добавление нового сообщения
        /// </summary>
        MessageAdd = 4,
        /// <summary>
        /// Друг стал онлайн
        /// </summary>
        FriendOnline = 8,
        /// <summary>
        /// Друг стал оффлайн
        /// </summary>
        FriendOffline = 9,
        /// <summary>
        /// Один из параметров (состав, тема) беседы был изменен
        /// </summary>
        ConversationChange = 51,
        /// <summary>
        /// Пользователь начал набирать текст в диалоге
        /// </summary>
        DialogUserTyping = 61,
        /// <summary>
        /// Пользователь начал набирать текст в беседе
        /// </summary>
        ConsersationUserTyping = 62,
        /// <summary>
        /// Пользователь совершил звонок
        /// </summary>
        UserCall = 70
    }

    /// <summary>
    /// Флаги сообщений, возвращаемых LongPoll-сервером
    /// </summary>
    [Flags]
    public enum VkLongPollMessageFlags
    {
        /// <summary>
        /// Сообщение не прочитано
        /// </summary>
        Unread = 1,
        /// <summary>
        /// Исходящее сообщение
        /// </summary>
        Outbox = 2,
        /// <summary>
        /// На сообщение был создан ответ
        /// </summary>
        Replied = 4,
        /// <summary>
        /// Помеченное сообщение
        /// </summary>
        Important = 8,
        /// <summary>
        /// Сообщение отправлено через чат
        /// </summary>
        Chat = 16,
        /// <summary>
        /// Сообщение отправлено другом
        /// </summary>
        Friends = 32,
        /// <summary>
        /// Сообщение помечено как спам
        /// </summary>
        Spam = 64,
        /// <summary>
        /// Сообщение удалено (в корзине)
        /// </summary>
        Deleted = 128,
        /// <summary>
        /// Сообщение проверено пользователем на спам
        /// </summary>
        Fixed = 256,
        /// <summary>
        /// Сообщение содержит медиаконтент
        /// </summary>
        Media = 512
    }

    /// <summary>
    /// Message returned by long poll service
    /// <seealso cref="http://vk.com/dev/using_longpoll"/>
    /// </summary>
    public class VkLongPollMessage
    {
        private const int ChatIdMask = 2000000000;
        public VkLongPollMessageType Type { get; internal set; }

        public Dictionary<string, object> Parameters { get; internal set; }

        public static VkLongPollMessage FromJson(JArray json)
        {
            var result = new VkLongPollMessage();
            result.Parameters = new Dictionary<string, object>();

            var messageType = json[0].Value<string>();

            switch (messageType)
            {
                //удаление сообщения с указанным local_id
                case "0":
                    result.Type = VkLongPollMessageType.MessageDelete;
                    result.Parameters.Add("message_id", json[1].Value<string>());
                    break;

                //замена флагов сообщения
                case "1":
                    result.Type = VkLongPollMessageType.MessageFlagUpdate;
                    result.Parameters.Add("message_id", json[1].Value<string>());
                    result.Parameters.Add("flags", json[2].Value<VkLongPollMessageFlags>());
                    break;

                //установка флагов сообщения
                case "2":
                    result.Type = VkLongPollMessageType.MessageFlagSet;
                    result.Parameters.Add("message_id", json[1].Value<string>());
                    result.Parameters.Add("flags", json[2].Value<int>());
                    if (json.Count > 2)
                        result.Parameters.Add("user_id", json[3].Value<long>());
                    break;

                //сброс флагов сообщения
                case "3":
                    result.Type = VkLongPollMessageType.MessageFlagReset;
                    result.Parameters.Add("message_id", json[1].Value<string>());
                    result.Parameters.Add("flags", json[2].Value<int>());
                    if (json.Count > 2)
                        result.Parameters.Add("user_id", json[3].Value<long>());
                    break;

                //добавление нового сообщения
                case "4":
                    result.Type = VkLongPollMessageType.MessageAdd;
                    var m = new VkMessage();
                    m.Id = json[1].Value<long>();
                    var flags = json[2].Value<int>();
                    var uid = json[3].Value<long>();
                    if (uid >= ChatIdMask)
                    {
                        //беседа
                        result.Parameters.Add("conversation", "1");
                        m.ChatId = (uid - ChatIdMask);
                    }
                    else
                    {
                        m.UserId = uid;
                    }

                    m.Date = DateTimeExtensions.UnixTimeStampToDateTime(json[4].Value<double>()).ToLocalTime();
                    m.Title = json[5].Value<string>();
                    m.Body = json[6].Value<string>();
                    result.Parameters.Add("message", m);
                    result.Parameters.Add("flags", flags);

                    if (json[7] != null)
                    {
                        //attachments

                        try
                        {
                            var longPollAttachments = new List<VkLongPollAttachment>();

                            var dict = json[7].Value<JObject>().Properties().ToDictionary(p => p.Name, p => p.Value.Value<string>());

                            foreach (var o in dict)
                            {
                                var id = o.Key.Replace("_type", string.Empty);

                                VkLongPollAttachment longPollAttachment = longPollAttachments.FirstOrDefault(a => a.Id == id);
                                if (longPollAttachment == null)
                                {
                                    longPollAttachment = new VkLongPollAttachment() { Id = id };
                                    longPollAttachments.Add(longPollAttachment);
                                }

                                if (o.Key.EndsWith("_type"))
                                    longPollAttachment.Type = o.Value;
                                else
                                {
                                    if (o.Key == "geo") //special attachment
                                    {
                                        longPollAttachment.Type = "geo";
                                    }
                                    else if (o.Key == "fwd")
                                    {
                                        longPollAttachment.Type = "fwd";
                                    }
                                    else if (o.Key == "from")
                                    {
                                        m.UserId = long.Parse(o.Value);
                                    }


                                    longPollAttachment.Data = o.Value;
                                }
                            }

                            if (longPollAttachments.Count > 0)
                            {
                                m.Attachments = new List<VkAttachment>(longPollAttachments.Count);

                                foreach (var longPollAttachment in longPollAttachments)
                                {
                                    if (longPollAttachment.Type == "geo")
                                    {
                                        //not an attachment for message
                                        //requires reverse geocoding
                                        m.Geo = new VkGeo();
                                    }
                                    else if (longPollAttachment.Id == "fwd_msg_count")
                                    {
                                        //adding placeholder messages, client app will reload message to get it
                                        int count = int.Parse(longPollAttachment.Data);
                                        m.ForwardMessages = new List<VkMessage>();
                                        for (int i = 0; i < count; i++)
                                            m.ForwardMessages.Add(new VkMessage());
                                    }
                                    else if (longPollAttachment.Type == "sticker")
                                    {
                                        var sticker = new VkStickerAttachment();

                                        sticker.ProductId = long.Parse(longPollAttachment.Data);
                                        sticker.Photo256 = "http://vk.com/images/stickers/" + sticker.ProductId + "/256.png";
                                        m.Attachments.Add(sticker);
                                    }
                                    else
                                    {
                                        var a = longPollAttachment.ToAttachment();
                                        if (a != null)
                                            m.Attachments.Add(a);
                                        else
                                            Debug.WriteLine("Unable to parse attachment " + a);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("Unable to parse attachments of LongPollMessage. " + ex);
                        }

                    }
                    break;

                //друг стал онлайн
                case "8":
                    result.Type = VkLongPollMessageType.FriendOnline;
                    result.Parameters.Add("user_id", json[1].Value<string>().Substring(1));
                    break;

                //друг стал оффлайн
                case "9":
                    result.Type = VkLongPollMessageType.FriendOffline;
                    result.Parameters.Add("user_id", json[1].Value<string>().Substring(1));
                    break;

                //один из параметров (состав, тема) беседы был изменен
                case "51":
                    result.Type = VkLongPollMessageType.ConversationChange;
                    result.Parameters.Add("chat_id", json[1].Value<string>());
                    result.Parameters.Add("self", json[2].Value<string>() == "1");
                    break;

                //пользователь начал набирать текст в диалоге
                case "61":
                    result.Type = VkLongPollMessageType.DialogUserTyping;
                    result.Parameters.Add("user_id", json[1].Value<long>());
                    break;
                //пользователь начал набирать текст в беседе
                case "62":
                    result.Type = VkLongPollMessageType.ConsersationUserTyping;
                    result.Parameters.Add("user_id", json[1].Value<long>());
                    result.Parameters.Add("chat_id", json[2].Value<long>());
                    break;

                default:
                    result.Type = VkLongPollMessageType.Unknown;
                    break;
            }

            return result;
        }
    }
}
