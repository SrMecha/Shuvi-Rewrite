using Shuvi.Classes.Data.Shop;
using Shuvi.Enums.Image;
using Shuvi.Interfaces.Customization;
using Shuvi.Interfaces.Shop.Products;
using Shuvi.Services.StaticServices.Database;

namespace Shuvi.Classes.Types.Shop.Products
{
    public class CustomizationProduct : Product, ICustomizationProduct
    {
        public string URL { get; init; }
        public ImageType Type { get; init; }

        public CustomizationProduct(ShopProductData data) : base(data)
        {
            URL = ImageDatabase.GetImage(Id).URL;
            Type = ImageDatabase.GetImage(Id).Type;
        }
        public IImage GetImage()
        {
            return ImageDatabase.GetImage(Id);
        }
    }
}
