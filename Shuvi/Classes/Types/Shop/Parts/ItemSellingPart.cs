using Shuvi.Interfaces.Inventory;
using Shuvi.Interfaces.Shop;
using Shuvi.Interfaces.Shop.Parts;
using Shuvi.Interfaces.Shop.Products;

namespace Shuvi.Classes.Types.Shop.Parts
{
    public class ItemSellingPart : Part<IItemProduct>, IItemSellingPart
    {
        public ItemSellingPart(IShopBasket basket, List<IItemProduct> products)
            : base(basket, products)
        {

        }
        public bool CanSell(IUserInventory userInventory, int page, int arrowIndex, int amount = 1)
        {
            if (_products.Count < 1)
                return false;
            var product = GetProduct(page, arrowIndex);
            return userInventory.GetItemAmount(product.Id) + _basket.GetItemAmount(product) >= amount * product.Amount;
        }
        public int GetMaxSell(IUserInventory userInventory, int page, int arrowIndex)
        {
            if (_products.Count < 1)
                return 0;
            var product = GetProduct(page, arrowIndex);
            return (userInventory.GetItemAmount(product.Id) + _basket.GetItemAmount(product)) / product.Amount;
        }
        public void Sell(int page, int arrowIndex, int amount = 1)
        {
            _basket.SellItem(GetProduct(page, arrowIndex), amount);
        }
    }
}
