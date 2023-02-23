using Shuvi.Classes.Data.Shop;
using Shuvi.Classes.Types.Localization;
using Shuvi.Classes.Types.Shop.Parts;
using Shuvi.Classes.Types.Shop.Products;
using Shuvi.Interfaces.Localization;
using Shuvi.Interfaces.Shop;
using Shuvi.Interfaces.Shop.Parts;
using Shuvi.Interfaces.Shop.Products;
using Shuvi.Interfaces.User;

namespace Shuvi.Classes.Types.Shop
{
    public class ShopBase : IShop
    {
        public ILocalizedInfo Info { get; }
        public IShopBasket ShopBasket { get; }
        public IItemSellingPart Selling { get; }
        public IItemPurchasingPart Purchasing { get; }
        public ICustomizationPart Customization { get; }

        public ShopBase(ShopData data)
        {
            Info = new LocalizedInfo(data.Name, data.Description);
            ShopBasket = new ShopBasket();
            Selling = new ItemSellingPart(ShopBasket, ConfigureItemProducts(data.UserSale));
            Purchasing = new ItemPurchasingPart(ShopBasket, ConfigureItemProducts(data.UserBuy));
            Customization = new CustomizationPart(ShopBasket, ConfigureCustomizationProducts(data.Customization));
;        }
        private List<IItemProduct> ConfigureItemProducts(List<ShopProductData> data)
        {
            var result = new List<IItemProduct>();
            foreach (var productData in data)
                result.Add(new ItemProduct(productData));
            return result;
        }
        private List<ICustomizationProduct> ConfigureCustomizationProducts(List<ShopProductData> data)
        {
            var result = new List<ICustomizationProduct>();
            foreach (var productData in data)
                result.Add(new CustomizationProduct(productData));
            return result;
        }
        public void Reset()
        {
            ShopBasket.Clear();
        }
        public async Task Confirm(IDatabaseUser user)
        {
            throw new NotImplementedException();
        }
    }
}
