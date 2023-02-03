using Shuvi.Enums.Localization;

namespace Shuvi.Interfaces.Localization
{
    public interface ILocalizedInfo
    {
        public string GetName(Language lang);
        public string GetDescription(Language lang);
    }
}
