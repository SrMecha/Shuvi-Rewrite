using Discord;
using MongoDB.Bson;
using Shuvi.Enums.Image;
using Shuvi.Enums.User;
using Shuvi.Interfaces.Customization;
using Shuvi.Services.StaticServices.Database;

namespace Shuvi.Classes.Types.Customization
{
    public class UserCustomization : IUserCustomization
    {
        protected List<ObjectId> _imagesCache;

        public Color Color { get; private set; }
        public IImage? Avatar { get; private set; }
        public IImage? Banner { get; private set; }
        public List<IImage> Avatars { get; private set; } = new();
        public List<IImage> Banners { get; private set; } = new();
        public UserBadges Badges { get; private set; }

        public UserCustomization(uint color, ObjectId? avatarId, ObjectId? bannerId, List<ObjectId> images, UserBadges bages)
        {
            Color = new Color(color);
            Avatar = avatarId is null ? null : ImageDatabase.GetImage((ObjectId)avatarId);
            Banner = bannerId is null ? null : ImageDatabase.GetImage((ObjectId)bannerId);
            _imagesCache = images;
            InitImages(images);
            Badges = bages;
        }
        protected void InitImages(List<ObjectId> images)
        {
            foreach (var id in images)
            {
                var image = ImageDatabase.GetImage(id);
                switch (image.Type)
                {
                    case ImageType.Avatar:
                        Avatars.Add(image);
                        break;
                    case ImageType.Banner:
                        Banners.Add(image);
                        break;
                }
            }
        }
        public void SetColor(uint hex)
        {
            Color = new Color(hex);
        }
        public void AddBadge(UserBadges bage)
        {
            Badges |= bage;
        }
        public void RemoveBadge(UserBadges bage)
        {
            Badges &= bage;
        }
        public void SetImage(ImageType type, ObjectId? id)
        {
            switch (type)
            {
                case ImageType.Avatar:
                    Avatar = id is null ? null : ImageDatabase.GetImage((ObjectId)id);
                    break;
                case ImageType.Banner:
                    Banner = id is null ? null : ImageDatabase.GetImage((ObjectId)id);
                    break;
            }
        }
        public void AddImage(ImageType type, ObjectId id)
        {
            _imagesCache.Add(id);
            switch (type)
            {
                case ImageType.Avatar:
                    Avatars.Add(ImageDatabase.GetImage(id));
                    break;
                case ImageType.Banner:
                    Banners.Add(ImageDatabase.GetImage(id));
                    break;
            }
        }
        public void RemoveImage(ImageType type, ObjectId id)
        {
            _imagesCache.Remove(id);
            switch (type)
            {
                case ImageType.Avatar:
                    Avatars.Remove(ImageDatabase.GetImage(id));
                    break;
                case ImageType.Banner:
                    Banners.Remove(ImageDatabase.GetImage(id));
                    break;
            }
        }
        public IEnumerable<IImage> GetAvatars()
        {
            return Avatars.AsEnumerable();
        }
        public IEnumerable<IImage> GetBanners()
        {
            return Banners.AsEnumerable();
        }
        public void AddImages(Dictionary<ImageType, ObjectId> images)
        {
            foreach (var (type, id) in images)
                AddImage(type, id);
        }
        public void AddImages(Dictionary<ImageType, List<ObjectId>> images)
        {
            foreach (var (type, ids) in images)
                foreach (var id in ids)
                    AddImage(type, id);
        }
        public List<ObjectId> GetImagesCache()
        {
            return _imagesCache;
        }
    }
}
