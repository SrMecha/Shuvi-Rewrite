using Shuvi.Interfaces.Inventory;
using Shuvi.Interfaces.Shop;
using Shuvi.Interfaces.Shop.Parts;
using Shuvi.Interfaces.Shop.Products;
using Shuvi.Interfaces.User;

namespace Shuvi.Classes.Types.Shop.Parts
{
    public class ItemPurchasingPart : Part<IItemProduct>, IItemPurchasingPart
    {

        public ItemPurchasingPart(IShopBasket basket, List<IItemProduct> products)
            : base(basket, products)
        {

        }
        public void Buy(int page, int arrowIndex, int amount = 1)
        {
            _basket.BuyItem(GetProduct(page, arrowIndex), amount);
        }
        public bool CanBuy(IUserInventory userInventory, IUserWallet wallet, int page, int arrowIndex, int amount = 1)
        {
            if (_products.Count < 1)
                return false;
            var product = GetProduct(page, arrowIndex);
            var item = userInventory.GetItem(product.Id);
            return item.Max <= userInventory.GetItemAmount(product.Id) + product.Amount * amount &&
                wallet.Get(product.MoneyType) + _basket.Wallet.Get(product.MoneyType) >= product.Price * amount;
        }
    }
}
