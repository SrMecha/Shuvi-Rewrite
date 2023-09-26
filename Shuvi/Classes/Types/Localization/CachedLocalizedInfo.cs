using Shuvi.Enums.Localization;
using Shuvi.Interfaces.Localization;
using Shuvi.Services.StaticServices.Localization;

namespace Shuvi.Classes.Types.Localization
{
    public class CachedLocalizedInfo : ILocalizedInfo
    {
        private readonly string _part;
        private readonly string _id;


        public CachedLocalizedInfo(string part, string? id = null)
        {
            _part = part;
            _id = id is null || id == string.Empty ? "None" : id;
        }
        public string GetName(Language lang)
        {
            return LocalizationService.Get(_part).Get(lang).Get($"{_id}/Name");
        }
        public string GetDescription(Language lang)
        {
            return LocalizationService.Get(_part).Get(lang).Get($"{_id}/Description");
        }
    }
}
