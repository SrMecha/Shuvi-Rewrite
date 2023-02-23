using Shuvi.Enums.Image;
using Shuvi.Interfaces.Customization;

namespace Shuvi.Interfaces.Shop.Products
{
    public interface ICustomizationProduct : IProduct
    {
        public string URL { get; }
        public ImageType Type { get; }

        public IImage GetImage();
    }
}
