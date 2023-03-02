using Discord;
using MongoDB.Bson;
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
        public UserBadges Badges { get; }

        public void AddImage(ImageType type, ObjectId id);
        public void AddImages(Dictionary<ImageType, ObjectId> images);
        public void AddImages(Dictionary<ImageType, List<ObjectId>> images);
        public void RemoveImage(ImageType type, ObjectId id);
        public void SetImage(ImageType type, ObjectId id);
        public void SetColor(uint hex);
        public void AddBadge(UserBadges bage);
        public void RemoveBadge(UserBadges bage);
        public IEnumerable<IImage> GetBanners();
        public IEnumerable<IImage> GetAvatars();
        public List<ObjectId> GetImagesCache();
    }
}
