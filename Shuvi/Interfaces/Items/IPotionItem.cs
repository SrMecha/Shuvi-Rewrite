using Shuvi.Enums.Characteristic;
using Shuvi.Enums.Localization;
using Shuvi.Interfaces.User;

namespace Shuvi.Interfaces.Items
{
    public interface IPotionItem : IItem
    {
        public Dictionary<DynamicCharacteristic, int> PotionRecover { get; init; }
        public string GetRecoverInfo(Language lang);
        public Task Use(IDatabaseUser dbUser);
    }
}
