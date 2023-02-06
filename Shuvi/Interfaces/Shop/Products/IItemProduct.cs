using Shuvi.Interfaces.Items;

namespace Shuvi.Interfaces.Shop.Products
{
    public interface IItemProduct : IProduct
    {
        public int Amount { get; }

        public IItem GetItem();
    }
}
