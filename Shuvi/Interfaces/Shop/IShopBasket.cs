using MongoDB.Bson;
using Shuvi.Enums.Image;
using Shuvi.Enums.Localization;
using Shuvi.Interfaces.Shop.Products;
using Shuvi.Interfaces.User;

namespace Shuvi.Interfaces.Shop
{
    public interface IShopBasket
    {
        public IUserWallet Wallet { get; }
        public Dictionary<ObjectId, int> Items { get; }
        public Dictionary<ImageType, List<ObjectId>> Customization { get; }
        public Language Language { get; }

        public void BuyCustomization(ICustomizationProduct product);
        public bool HaveCustomization(ICustomizationProduct product);
        public void BuyItem(IItemProduct product, int amount);
        public void SellItem(IItemProduct product, int amount);
        public int GetItemAmount(IItemProduct product);
        public string GetItemsInfo();
        public string GetCustomizationInfo();
        public void Clear();
        public IEnumerator<IItemProduct> GetItemProducts();
        public IEnumerator<ICustomizationProduct> GetCustomizationProducts();
    }
}
