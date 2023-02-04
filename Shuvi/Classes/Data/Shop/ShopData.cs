using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shuvi.Enums.Localization;
using Shuvi.Enums.Shop;

namespace Shuvi.Classes.Data.Shop
{
    public sealed class ShopData
    {
        [BsonId]
        public ObjectId Id { get; set; } = ObjectId.Empty;
        public ShopType Type { get; set; } = ShopType.Simple;
        public Dictionary<Language, string> Name { get; set; } = new();
        public Dictionary<Language, string> Description { get; set; } = new();
        public List<ShopProductData> UserSale { get; set; } = new();
        public List<ShopProductData> UserBuy { get; set; } = new();
    }
}
