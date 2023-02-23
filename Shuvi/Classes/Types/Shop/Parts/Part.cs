using Discord;
using Shuvi.Enums.Localization;
using Shuvi.Interfaces.Shop;
using Shuvi.Interfaces.Shop.Parts;
using Shuvi.Interfaces.Shop.Products;

namespace Shuvi.Classes.Types.Shop.Parts
{
    public class Part<TProduct> : IPart<TProduct>
        where TProduct : IProduct
    {
        protected List<TProduct> _products;
        protected IShopBasket _basket;

        public Part(IShopBasket basket, List<TProduct> products)
        {
            _basket = basket;
            _products = products;
        }
        public TProduct GetProduct(int itemIndex)
        {
            return _products[itemIndex];
        }
        public TProduct GetProduct(int page, int arrowIndex)
        {
            return _products[page * 10 + arrowIndex];
        }
        public int GetTotalPages()
        {
            return _products.Count / 10;
        }
        public bool HaveProducts()
        {
            return _products.Count > 0;
        }
        public IEnumerable<TProduct> GetProductsInPage(int page)
        {
            for (int i = page * 10; i < _products.Count && i < page * 10 + 10; i++)
                yield return _products[i];
        }
    }
}
