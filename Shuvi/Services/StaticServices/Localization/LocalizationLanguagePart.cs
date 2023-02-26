using Shuvi.Enums.Localization;

namespace Shuvi.Services.StaticServices.Localization
{
    public class LocalizationLanguagePart
    {
        public List<LocalizationKeyPart> Langs { get; private set; } = new();

        public LocalizationLanguagePart(List<LocalizationKeyPart> langs)
        {
            Langs = langs;
        }
        public LocalizationLanguagePart(List<Dictionary<string, string>> langs)
        {
            foreach (var lang in langs)
                Langs.Add(new(lang));
        }
        public LocalizationLanguagePart()
        {

        }
        public LocalizationKeyPart Get(Language lang)
        {
            return (int)lang < Langs.Count ? Langs.ElementAt((int)lang) : new();
        }
    }
}
