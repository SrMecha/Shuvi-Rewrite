using Shuvi.Classes.Data.Shop;
using Shuvi.Interfaces.Items;
using Shuvi.Interfaces.Shop.Products;
using Shuvi.Services.StaticServices.Database;

namespace Shuvi.Classes.Types.Shop.Products
{
    public class ItemProduct : Product, IItemProduct
    {
        public int Amount { get; init; }

        public ItemProduct(ShopProductData data) : base(data)
        {
            Amount = data.Amount;
        }
        public IItem GetItem()
        {
            return ItemDatabase.GetItem(Id);
        }
    }
}
