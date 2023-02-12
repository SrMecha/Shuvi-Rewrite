using MongoDB.Bson;
using Shuvi.Classes.Data.Customization;
using Shuvi.Classes.Types.Customization;
using Shuvi.Enums.Image;
using Shuvi.Enums.Localization;
using Shuvi.Interfaces.Customization;

namespace Shuvi.Classes.Factories.Image
{
    public static class ImageFactory
    {
        public static IImage CreateImage(ImageData data)
        {
            return new CustomizationImage(data);
        }
        public static IImage CreateImage(ObjectId id, Dictionary<Language, string> names, ImageType type, string url)
        {
            return new CustomizationImage(id, names, type, url);
        }
    }
}
