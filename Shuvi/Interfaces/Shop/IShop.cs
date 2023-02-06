using Shuvi.Enums.Localization;
using Shuvi.Interfaces.Localization;
using Shuvi.Interfaces.Shop.Parts;
using Shuvi.Interfaces.User;

namespace Shuvi.Interfaces.Shop
{
    public interface IShop
    {
        public Language Language { get; }
        public ILocalizedInfo Info { get; }
        public IShopBasket ShopBasket { get; }
        public IItemSellingPart Selling { get; }
        public IItemPurchasingPart Purchasing { get; }
        public ICustomizationPart Customization { get; }

        public void Reset();
        public Task Confirm(IDatabaseUser user);
    }
}
