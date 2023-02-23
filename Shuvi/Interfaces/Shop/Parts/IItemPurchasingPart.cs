using Shuvi.Interfaces.Inventory;
using Shuvi.Interfaces.Shop.Products;
using Shuvi.Interfaces.User;

namespace Shuvi.Interfaces.Shop.Parts
{
    public interface IItemPurchasingPart : IPart<IItemProduct>
    {
        public bool CanBuy(IUserInventory userInventory, IUserWallet wallet, int page, int arrowIndex, int amount = 1);
        public void Buy(int page, int arrowIndex, int amount = 1);
    }
}
