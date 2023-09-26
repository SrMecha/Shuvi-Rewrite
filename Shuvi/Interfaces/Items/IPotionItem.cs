using Shuvi.Enums.Localization;
using Shuvi.Interfaces.Characteristics.Bonuses;
using Shuvi.Interfaces.User;

namespace Shuvi.Interfaces.Items
{
    public interface IPotionItem : IItem
    {
        public IDynamicBonuses PotionRecover { get; init; }
        public string GetRecoverInfo(Language lang);
        public Task Use(IDatabaseUser dbUser);
    }
}
