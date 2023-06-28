using MongoDB.Bson;
using MongoDB.Driver;
using Shuvi.Classes.Data.Customization;
using Shuvi.Classes.Factories.Image;
using Shuvi.Classes.Types.Customization;
using Shuvi.Interfaces.Customization;

namespace Shuvi.Services.StaticServices.Database
{
    public static class ImageDatabase
    {
        private static IMongoCollection<ImageData>? _collection;
        private static Dictionary<ObjectId, IImage> _cache = new();

        public static List<IImage> Images { get; private set; } = new();

        public static void Init(IMongoCollection<ImageData> collection)
        {
            _collection = collection;
            LoadImages();
        }
        public static IImage GetImage(ObjectId id)
        {
            return _cache.GetValueOrDefault(id, new CustomizationImage());
        }
        private static void LoadImages()
        {
            foreach (var data in _collection.FindSync(new BsonDocument { }).ToEnumerable<ImageData>())
            {
                var image = ImageFactory.CreateImage(data);
                _cache.Add(data.Id, image);
                Images.Add(image);
            }
        }
    }
}

