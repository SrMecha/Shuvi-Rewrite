using MongoDB.Bson;
using Shuvi.Classes.Data.Customization;
using Shuvi.Classes.Settings;
using Shuvi.Classes.Types.Localization;
using Shuvi.Enums.Image;
using Shuvi.Enums.Localization;
using Shuvi.Interfaces.Customization;
using Shuvi.Interfaces.Localization;

namespace Shuvi.Classes.Types.Customization
{
    public class CustomizationImage : IImage
    {
        public ObjectId Id { get; private set; }
        public ILocalizedInfo Info { get; private set; }
        public ImageType Type { get; private set; }
        public string URL { get; private set; }

        public CustomizationImage()
        {
            Id = ObjectId.Empty;
            Info = new LocalizedInfo(new());
            Type = ImageType.Avatar;
            URL = ImageSettings.ImageNotFoundURL;
        }
        public CustomizationImage(ImageData data) 
        {
            Id = data.Id;
            Info = new LocalizedInfo(data.Name);
            Type = data.Type;
            URL = data.URL;
        }
        public CustomizationImage(ObjectId id, Dictionary<Language, string> names, ImageType type, string url)
        {
            Id = id;
            Info = new LocalizedInfo(names);
            Type = type;
            URL = url;
        }
    }
}
