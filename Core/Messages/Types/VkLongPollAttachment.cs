using System;
using VkLib.Core.Attachments;

namespace VkLib.Core.Messages
{
    internal class VkLongPollAttachment
    {
        public string Id { get; set; }

        public string Type { get; set; }

        public string Data { get; set; }

        public VkAttachment ToAttachment()
        {
            var dataArray = Data.Split(new string[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
            if (dataArray.Length != 2)
            {
                //invalid data format
                return null;
            }

            long ownerId = 0;
            long.TryParse(dataArray[0], out ownerId);

            long itemId = 0;
            long.TryParse(dataArray[1], out itemId);


            switch (Type)
            {
                case "audio":
                    return new VkAudioAttachment()
                    {
                        Id = itemId,
                        OwnerId = ownerId
                    };

                case "photo":
                    return new VkPhotoAttachment()
                    {
                        Id = itemId,
                        OwnerId = ownerId
                    };

                case "sticker":
                    return new VkStickerAttachment()
                    {
                        Id = itemId,
                        OwnerId = ownerId
                    };

                case "gift":
                    return new VkGiftAttachment()
                    {
                        Id = itemId,
                        OwnerId = ownerId
                    };

                case "link":
                    return new VkLinkAttachment()
                    {
                        Id = itemId,
                        OwnerId = ownerId
                    };


                case "doc":
                    return new VkDocumentAttachment()
                    {
                        Id = itemId,
                        OwnerId = ownerId
                    };

                case "video":
                    return new VkVideoAttachment()
                    {
                        Id = itemId,
                        OwnerId = ownerId
                    };

                case "wall":
                    return new VkWallPostAttachment()
                    {
                        Id = itemId,
                        OwnerId = ownerId
                    };
            }

            return null;
        }

        public override string ToString()
        {
            return string.Format("{0}_{1}", Type, Data);
        }
    }
}