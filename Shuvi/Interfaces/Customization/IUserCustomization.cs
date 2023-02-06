using Discord;
using MongoDB.Bson;
using Shuvi.Classes.Data.User;
using Shuvi.Enums.Image;
using Shuvi.Enums.User;

namespace Shuvi.Interfaces.Customization
{
    public interface IUserCustomization
    {
        public Color Color { get; set; }
        public IImage Avatar { get; set; }
        public IImage Banner { get; set; }
        public List<UserImageData> Avatars { get; set; }
        public List<UserImageData> Banners { get; set; }
        public UserBages Bages { get; set; }

        public void AddImage(ImageType type, ObjectId id);
        public void RemoveImage(ImageType type, ObjectId id);
        public void SetImage(ImageType type, ObjectId id);
        public void SetColor(uint hex);
        public void AddBage(UserBages bage);
        public void RemoveBage(UserBages bage);
        public IEnumerable<IImage> GetBanners();
        public IEnumerable<IImage> GetAvatars();
    }
}
