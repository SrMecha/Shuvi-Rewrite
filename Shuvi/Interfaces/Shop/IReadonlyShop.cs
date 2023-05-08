using MongoDB.Bson;
using Shuvi.Interfaces.Localization;

namespace Shuvi.Interfaces.Shop
{
    public interface IReadonlyShop
    {
        public ObjectId Id { get; }
        public ILocalizedInfo Info { get; }

        public IShop CreateShop();
    }
}
