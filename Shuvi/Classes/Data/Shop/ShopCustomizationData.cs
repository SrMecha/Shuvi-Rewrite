using MongoDB.Bson;
using Shuvi.Enums.Money;

namespace Shuvi.Classes.Data.Shop
{
    public class ShopCustomizationData
    {
        public ObjectId Id { get; set; } = ObjectId.Empty;
        public MoneyType Type { get; set; } = MoneyType.Gold;
        public int Price { get; set; } = 0;
    }
}
