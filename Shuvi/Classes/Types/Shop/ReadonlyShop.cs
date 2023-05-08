using MongoDB.Bson;
using Shuvi.Classes.Data.Shop;
using Shuvi.Classes.Types.Localization;
using Shuvi.Interfaces.Localization;
using Shuvi.Interfaces.Shop;

namespace Shuvi.Classes.Types.Shop
{
    public class ReadonlyShop : IReadonlyShop
    {
        public ObjectId Id { get; init; }
        public ILocalizedInfo Info { get; init; }
        public ShopData Data { get; init; }

        public ReadonlyShop(ShopData data)
        {
            Id = data.Id;
            Info = new LocalizedInfo(data.Name, data.Description);
            Data = data;
        }

        public IShop CreateShop()
        {
            return new ShopBase(Data);
        }
    }
}
