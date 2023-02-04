using MongoDB.Bson;
using Shuvi.Enums.Image;
using Shuvi.Interfaces.Localization;

namespace Shuvi.Interfaces.Customization
{
    public interface IImage
    {
        public ObjectId Id { get; }
        public ILocalizedInfo Info { get; }
        public ImageType Type { get; }
    }
}
