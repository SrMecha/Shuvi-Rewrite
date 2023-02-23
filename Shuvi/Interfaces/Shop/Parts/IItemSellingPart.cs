using Shuvi.Interfaces.Inventory;
using Shuvi.Interfaces.Shop.Products;

namespace Shuvi.Interfaces.Shop.Parts
{
    public interface IItemSellingPart : IPart<IItemProduct>
    {
        public bool CanSell(IUserInventory userInventory, int page, int arrowIndex, int amount = 1);
        public void Sell(int page, int arrowIndex, int amount = 1);
        public int GetMaxSell(IUserInventory userInventory, int page, int arrowIndex);
    }
}
