using Discord;
using Shuvi.Enums.Localization;
using Shuvi.Interfaces.Shop.Products;

namespace Shuvi.Interfaces.Shop.Parts
{
    public interface IPart<TProduct> where TProduct: IProduct
    {
        public Language Language { get; }

        public string GetProductsInfo(int page, int arrowIndex);
        public string GetProductsInfo(int page);
        public List<SelectMenuOptionBuilder> GetSelectMenuOptions(int page);
        public TProduct GetProduct(int itemIndex);
        public TProduct GetProductItem(int page, int arrowIndex);
        public bool HaveProducts();
        public int GetTotalPages();
    }
}
