using MongoDB.Bson;
using Shuvi.Enums.Money;

namespace Shuvi.Interfaces.Shop.Products
{
    public interface IProduct
    {
        public ObjectId Id { get; }
        public MoneyType MoneyType { get; }
        public int Price { get; }
    }
}
