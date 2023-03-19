using Discord;
using MongoDB.Driver;
using Shuvi.Classes.Data.Item;
using Shuvi.Classes.Data.User;
using Shuvi.Classes.Extensions;
using Shuvi.Enums.Characteristic;
using Shuvi.Enums.Localization;
using Shuvi.Interfaces.Items;
using Shuvi.Interfaces.User;
using Shuvi.Services.StaticServices.Database;
using Shuvi.Services.StaticServices.Localization;

namespace Shuvi.Classes.Types.Item
{
    public class PotionItem : SimpleItem, IPotionItem
    {
        public Dictionary<DynamicCharacteristic, int> PotionRecover { get; init; }

        public PotionItem(ItemData data) : base(data)
        {
            PotionRecover = data.PotionRecover ?? new();
        }
        public string GetRecoverInfo(Language lang)
        {
            var result = new List<string>();
            foreach (var (characteristic, amount) in PotionRecover)
                result.Add($"{LocalizationService.Get("names").Get(lang).Get(characteristic.GetLowerName())} {(amount > 0 ? $"+{amount}" : amount)}");
            if (result.Count < 1)
                return LocalizationService.Get("names").Get(lang).Get("notHave");
            return string.Join("\n", result);
        }
        public async Task Use(IDatabaseUser dbUser)
        {
            dbUser.Characteristics.Energy.Add(PotionRecover.GetValueOrDefault(DynamicCharacteristic.Energy, 0));
            dbUser.Characteristics.Mana.Add(PotionRecover.GetValueOrDefault(DynamicCharacteristic.Mana, 0));
            dbUser.Characteristics.Health.Add(PotionRecover.GetValueOrDefault(DynamicCharacteristic.Health, 0));
            await UserDatabase.UpdateUser(dbUser.Id, new UpdateDefinitionBuilder<UserData>()
                .Set(x => x.EnergyRegenTime, dbUser.Characteristics.Energy.RegenTime)
                .Set(x => x.ManaRegenTime, dbUser.Characteristics.Mana.RegenTime)
                .Set(x => x.HealthRegenTime, dbUser.Characteristics.Health.RegenTime));
        }
    }
}

