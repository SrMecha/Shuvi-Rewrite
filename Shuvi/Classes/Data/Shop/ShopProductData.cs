using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shuvi.Enums.Money;

namespace Shuvi.Classes.Data.Shop
{
    [BsonNoId]
    public sealed class ShopProductData
    {
        public ObjectId Id { get; set; } = ObjectId.Empty;
        public MoneyType Type { get; set; } = MoneyType.Gold;
        public int Price { get; set; } = 0;
        public int Amount { get; set; } = 1;
    }
}
