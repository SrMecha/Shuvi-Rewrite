using MongoDB.Bson;
using Shuvi.Classes.Types.User;
using Shuvi.Enums.Image;
using Shuvi.Interfaces.Customization;
using Shuvi.Interfaces.Items;
using Shuvi.Interfaces.Shop;
using Shuvi.Interfaces.Shop.Products;
using Shuvi.Interfaces.User;
using Shuvi.Services.StaticServices.Database;

namespace Shuvi.Classes.Types.Shop
{
    public class ShopBasket : IShopBasket
    {
        public IUserWallet Wallet { get; private set; }
        public Dictionary<ObjectId, int> Items { get; private set; }
        public Dictionary<ImageType, List<ObjectId>> Customization { get; private set; }

        public ShopBasket()
        {
            Wallet = new UserWallet();
            Items = new();
            Customization = new()
            {
                {ImageType.Avatar, new() },
                {ImageType.Banner, new() }
            };
        }
        public void BuyCustomization(ICustomizationProduct product)
        {
            Customization[product.Type].Add(product.Id);
        }
        public void BuyItem(IItemProduct product, int amount)
        {
            if (Items.ContainsKey(product.Id))
                Items[product.Id] += amount * product.Amount;
            else
                Items[product.Id] = amount * product.Amount;
            Wallet.Reduce(product.MoneyType, product.Price * amount);
        }
        public void Clear()
        {
            Wallet = new UserWallet();
            Items = new();
            Customization = new();
        }
        public int GetItemAmount(IItemProduct product)
        {
            return Items.GetValueOrDefault(product.Id, 0);
        }
        public bool HaveCustomization(ICustomizationProduct product)
        {
            return Customization[product.Type].Contains(product.Id);
        }
        public void SellItem(IItemProduct product, int amount)
        {
            if (Items.ContainsKey(product.Id))
                Items[product.Id] -= amount * product.Amount;
            else
                Items[product.Id] = -amount * product.Amount;
            Wallet.Add(product.MoneyType, product.Price * amount);
        }
        public IEnumerable<(IItem, int)> GetItems()
        {
            foreach (var (id, amount) in Items)
                yield return (ItemDatabase.GetItem(id), amount);
        }
        public IEnumerable<IImage> GetCustomizations()
        {
            foreach (var list in Customization.Values)
                foreach (var id in list)
                    yield return ImageDatabase.GetImage(id);
        }
    }
}
