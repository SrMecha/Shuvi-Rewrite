using Discord;
using MongoDB.Bson;
using Shuvi.Classes.Data.User;
using Shuvi.Enums.Image;
using Shuvi.Enums.User;

namespace Shuvi.Interfaces.Customization
{
    public interface IUserCustomization
    {
        public Color Color { get; }
        public IImage? Avatar { get; }
        public IImage? Banner { get; }
        public List<IImage> Avatars { get; }
        public List<IImage> Banners { get; }
        public UserBages Bages { get; }

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
