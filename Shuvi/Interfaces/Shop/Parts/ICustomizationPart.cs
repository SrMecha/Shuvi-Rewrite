using Shuvi.Interfaces.Shop.Products;

namespace Shuvi.Interfaces.Shop.Parts
{
    public interface ICustomizationPart : IPart<ICustomizationProduct>
    {
        public bool CanBuy(int page, int arrowIndex);
        public void Buy(int page, int arrowIndex);
    }
}
