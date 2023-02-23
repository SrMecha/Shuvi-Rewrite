using MongoDB.Bson;
using Shuvi.Classes.Data.Shop;
using Shuvi.Enums.Money;
using Shuvi.Interfaces.Shop.Products;

namespace Shuvi.Classes.Types.Shop.Products
{
    public class Product : IProduct
    {
        public ObjectId Id { get; init; }
        public MoneyType MoneyType { get; init; }
        public int Price { get; init; }

        public Product(ShopProductData data)
        {
            Id = data.Id;
            MoneyType = data.Type;
            Price = data.Price;
        }
    }
}
