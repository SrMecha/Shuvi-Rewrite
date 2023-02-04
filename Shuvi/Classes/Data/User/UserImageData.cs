using MongoDB.Bson;
using Shuvi.Enums.Image;

namespace Shuvi.Classes.Data.User
{
    public sealed class UserImageData
    {
        public ObjectId Id { get; set; } = ObjectId.Empty;
        public ImageType Type { get; set; } = ImageType.Avatar;
    }
}
