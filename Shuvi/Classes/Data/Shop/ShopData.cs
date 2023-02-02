using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Shuvi.Enums.Shop;

namespace Shuvi.Classes.Data.Shop
{
    public sealed class ShopData
    {
        [BsonId]
        public ObjectId Id { get; set; } = ObjectId.Empty;
        public ShopType Type { get; set; } = ShopType.Simple;
        public string Name { get; set; } = "NoNameProvided";
        public string Description { get; set; } = "NoDescriptionProvided";
        public List<ShopProductData> UserSale { get; set; } = new();
        public List<ShopProductData> UserBuy { get; set; } = new();
    }
}
