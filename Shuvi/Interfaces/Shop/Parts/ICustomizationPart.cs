using Shuvi.Interfaces.Customization;
using Shuvi.Interfaces.Shop.Products;
using Shuvi.Interfaces.User;

namespace Shuvi.Interfaces.Shop.Parts
{
    public interface ICustomizationPart : IPart<ICustomizationProduct>
    {
        public bool CanBuy(IUserCustomization userCustomization, IUserWallet wallet, int page, int arrowIndex);
        public void Buy(int page, int arrowIndex);
    }
}
