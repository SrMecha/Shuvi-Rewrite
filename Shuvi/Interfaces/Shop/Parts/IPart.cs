using Shuvi.Interfaces.Shop.Products;

namespace Shuvi.Interfaces.Shop.Parts
{
    public interface IPart<TProduct> where TProduct : IProduct
    {
        public TProduct GetProduct(int itemIndex);
        public TProduct GetProduct(int page, int arrowIndex);
        public IEnumerable<TProduct> GetProductsInPage(int page);
        public bool HaveProducts();
        public int GetTotalPages();
    }
}
