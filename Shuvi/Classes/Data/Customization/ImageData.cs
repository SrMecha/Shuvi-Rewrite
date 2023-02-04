using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shuvi.Enums.Image;
using Shuvi.Enums.Localization;

namespace Shuvi.Classes.Data.Customization
{
    public class ImageData
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public Dictionary<Language, string> Name { get; set; } = new();
        public ImageType Type { get; set; } = ImageType.Avatar;
        public string URL { get; set; } = string.Empty;
    }
}
