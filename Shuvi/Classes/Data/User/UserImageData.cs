using MongoDB.Bson;
using Shuvi.Enums.Image;

namespace Shuvi.Classes.Data.User
{
    public class UserImageData
    {
        public ObjectId? Id { get; set; } = null;
        public ImageType Type { get; set; } = ImageType.Custom;
        public string? Name { get; set; } = null;
    }
}
