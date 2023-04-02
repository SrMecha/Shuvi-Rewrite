namespace Shuvi.Services.StaticServices.Localization
{
    public class LocalizationKeyPart
    {
        public Dictionary<string, string> Keys { get; private set; } = new();

        public LocalizationKeyPart(Dictionary<string, string> keys)
        {
            Keys = keys;
        }
        public LocalizationKeyPart()
        {

        }
        public string Get(string key)
        {
            return Keys.GetValueOrDefault(key, key);
        }
    }
}
