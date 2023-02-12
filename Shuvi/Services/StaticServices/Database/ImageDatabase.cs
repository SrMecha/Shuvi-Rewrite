using MongoDB.Bson;
using MongoDB.Driver;
using Shuvi.Classes.Data.Customization;
using Shuvi.Classes.Data.Item;
using Shuvi.Classes.Factories.Image;
using Shuvi.Classes.Types.Customization;
using Shuvi.Interfaces.Customization;

namespace Shuvi.Services.StaticServices.Database
{
    public static class ImageDatabase
    {
        private static IMongoCollection<ImageData>? _collection;
        private static Dictionary<ObjectId, IImage> _cache = new();

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
            _cache = new();
            foreach (var data in _collection.FindSync(new BsonDocument { }).ToEnumerable<ImageData>())
            {
                _cache.Add(data.Id, ImageFactory.CreateImage(data));
            }
        }
    }
}

