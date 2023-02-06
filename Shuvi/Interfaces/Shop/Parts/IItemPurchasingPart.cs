using Shuvi.Interfaces.Shop.Products;

namespace Shuvi.Interfaces.Shop.Parts
{
    public interface IItemPurchasingPart : IPart<IItemProduct>
    {
        public bool CanBuy(int page, int arrowIndex, int amount = 1);
        public void Buy(int page, int arrowIndex, int amount = 1);
    }
}
