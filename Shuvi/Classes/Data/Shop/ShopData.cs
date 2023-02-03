using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Shuvi.Enums.Shop;
using Shuvi.Enums.Localization;

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
