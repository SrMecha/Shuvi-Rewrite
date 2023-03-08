using Shuvi.Enums.Localization;
using Shuvi.Interfaces.Localization;

namespace Shuvi.Classes.Types.Localization
{
    public class LocalizedInfo : ILocalizedInfo
    {
        protected readonly Dictionary<Language, string> _names;
        protected readonly Dictionary<Language, string> _descriptions;

        public LocalizedInfo(Dictionary<Language, string> names, Dictionary<Language, string> descriptions)
        {
            _names = names;
            _descriptions = descriptions;
        }
        public LocalizedInfo(Dictionary<Language, string> names)
        {
            _names = names;
            _descriptions = new();
        }
        public string GetName(Language lang)
        {
            return _names.GetValueOrDefault(lang, "NoNameProvided");
        }
        public string GetDescription(Language lang)
        {
            return _descriptions.GetValueOrDefault(lang, "NoDescriptionProvided");
        }
    }
}
