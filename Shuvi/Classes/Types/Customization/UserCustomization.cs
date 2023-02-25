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
        public Color Color { get; private set; }
        public IImage? Avatar { get; private set; }
        public IImage? Banner { get; private set; }
        public List<IImage> Avatars { get; private set; } = new();
        public List<IImage> Banners { get; private set; } = new();
        public UserBages Bages { get; private set; }

        public UserCustomization(uint color, ObjectId? avatarId, ObjectId? bannerId, List<ObjectId> images, UserBages bages) 
        {
            Color = new Color(color);
            Avatar = avatarId is null ? null : ImageDatabase.GetImage((ObjectId)avatarId);
            Banner = bannerId is null ? null : ImageDatabase.GetImage((ObjectId)bannerId);
            InitImages(images);
            Bages = bages;
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
        public void AddBage(UserBages bage)
        {
            Bages |= bage;
        }
        public void RemoveBage(UserBages bage)
        {
            Bages &= bage;
        }
        public void SetImage(ImageType type, ObjectId id)
        {
            switch (type)
            {
                case ImageType.Avatar:
                    Avatar = ImageDatabase.GetImage(id);
                    break;
                case ImageType.Banner:
                    Banner = ImageDatabase.GetImage(id);
                    break;
            }
        }
        public void AddImage(ImageType type, ObjectId id)
        {
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
    }
}
