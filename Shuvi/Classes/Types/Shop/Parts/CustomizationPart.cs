using Shuvi.Enums.Image;
using Shuvi.Interfaces.Customization;
using Shuvi.Interfaces.Shop;
using Shuvi.Interfaces.Shop.Parts;
using Shuvi.Interfaces.Shop.Products;
using Shuvi.Interfaces.User;

namespace Shuvi.Classes.Types.Shop.Parts
{
    public class CustomizationPart : Part<ICustomizationProduct>, ICustomizationPart
    {
        public CustomizationPart(IShopBasket basket, List<ICustomizationProduct> products)
            : base(basket, products)
        {

        }
        public void Buy(int page, int arrowIndex)
        {
            _basket.BuyCustomization(GetProduct(page, arrowIndex));
        }
        public bool CanBuy(IUserCustomization userCustomization, IUserWallet wallet, int page, int arrowIndex)
        {
            if (_products.Count < 1)
                return false;
            var product = GetProduct(page, arrowIndex);
            var image = product.GetImage();
            return (product.Type == ImageType.Avatar ? !userCustomization.Avatars.Contains(image) : !userCustomization.Banners.Contains(image))
                && !_basket.HaveCustomization(product)
                && wallet.Get(product.MoneyType) + _basket.Wallet.Get(product.MoneyType) >= product.Price;
        }
    }
}
