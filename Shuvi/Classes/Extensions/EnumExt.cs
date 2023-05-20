using Discord;
using Shuvi.Enums.Characteristic;
using Shuvi.Enums.Item;
using Shuvi.Enums.Localization;
using Shuvi.Enums.Magic;
using Shuvi.Enums.Money;
using Shuvi.Enums.Rating;
using Shuvi.Enums.Requirements;
using Shuvi.Enums.User;
using Shuvi.Services.StaticServices.Emoji;
using System;
using System.Runtime.CompilerServices;

namespace Shuvi.Classes.Extensions
{
    public static class EnumExt
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static string GetName(this Rank target)
        {
            return target switch
            {
                Rank.E => nameof(Rank.E),
                Rank.D => nameof(Rank.D),
                Rank.C => nameof(Rank.C),
                Rank.B => nameof(Rank.B),
                Rank.A => nameof(Rank.A),
                Rank.S => nameof(Rank.S),
                Rank.SS => nameof(Rank.SS),
                Rank.SSS => nameof(Rank.SSS),
                _ => "Error"
            };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool CanRankUp(this Rank target)
        {
            return target != Rank.SSS;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static int GetNeedRating(this Rank target)
        {
            return target switch
            {
                Rank.E => 0,
                Rank.D => 100,
                Rank.C => 300,
                Rank.B => 600,
                Rank.A => 1000,
                Rank.S => 2000,
                Rank.SS => 3500,
                Rank.SSS => 5000,
                _ => -1
            };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static string GetLowerName(this BaseRequirement target)
        {
            return target switch
            {
                BaseRequirement.Strength => "strength",
                BaseRequirement.Agility => "agility",
                BaseRequirement.Luck => "luck",
                BaseRequirement.Intellect => "intellect",
                BaseRequirement.Endurance => "endurance",
                BaseRequirement.Rank => "rank",
                _ => string.Empty
            };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static string Format(this BaseRequirement target, int amount)
        {
            return target switch
            {
                BaseRequirement.Rank => ((Rank)amount).GetName(),
                _ => amount.ToString()
            };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static string GetLowerName(this DynamicCharacteristic target)
        {
            return target switch
            {
                DynamicCharacteristic.Health => "health",
                DynamicCharacteristic.Energy => "energy",
                DynamicCharacteristic.Mana => "mana",
                _ => string.Empty
            };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static string GetLowerName(this MagicType target)
        {
            return target switch
            {
                MagicType.None => "none",
                MagicType.Fire => "fire",
                MagicType.Water => "water",
                MagicType.Wind => "wind",
                MagicType.Earth => "earth",
                MagicType.Losen => "losen",
                _ => "none"
            };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static string GetLowerName(this StaticCharacteristic target)
        {
            return target switch
            {
                StaticCharacteristic.Strength => "strength",
                StaticCharacteristic.Agility => "agility",
                StaticCharacteristic.Luck => "luck",
                StaticCharacteristic.Intellect => "intellect",
                StaticCharacteristic.Endurance => "endurance",
                _ => string.Empty
            };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static string GetLowerName(this MoneyType target)
        {
            return target switch
            {
                MoneyType.Gold => "gold",
                MoneyType.Dispoints => "dispoints",
                _ => string.Empty
            };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static string GetLowerName(this UserRace target)
        {
            return target switch
            {
                UserRace.ExMachina => "exMachina",
                UserRace.Beastmen => "beastmen",
                _ => "none"
            };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Array GetRaceBreeds(this UserRace target)
        {
            return target switch
            {
                UserRace.Beastmen => new UserSubrace[] { UserSubrace.Werewolf },
                _ => new UserSubrace[] { UserSubrace.NoSubrace }
            };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static string GetLowerName(this UserSubrace target)
        {
            return target switch
            {
                UserSubrace.NoSubrace => "none",
                UserSubrace.Werewolf => "werewolf",
                _ => "none"
            };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static string GetLowerName(this UserProfession target)
        {
            return target switch
            {
                UserProfession.NoProfession => "none",
                UserProfession.Prufer => "prufer",
                UserProfession.Kampfer => "kampfer",
                _ => "none"
            };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static string GetLowerName(this ItemType target)
        {
            return target switch
            {
                ItemType.Simple => "simple",
                ItemType.Weapon => "weapon",
                ItemType.Helmet => "helmet",
                ItemType.Armor => "armor",
                ItemType.Leggings => "leggings",
                ItemType.Boots => "boots",
                ItemType.Potion => "potion",
                ItemType.Chest => "chest",
                ItemType.Amulet => "amulet",
                ItemType.Recipe => "recipe",
                _ => "none",
            };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static string GetNameOf(this ItemType target)
        {
            return target switch
            {
                ItemType.Simple => nameof(ItemType.Simple),
                ItemType.Weapon => nameof(ItemType.Weapon),
                ItemType.Helmet => nameof(ItemType.Helmet),
                ItemType.Armor => nameof(ItemType.Armor),
                ItemType.Leggings => nameof(ItemType.Leggings),
                ItemType.Boots => nameof(ItemType.Boots),
                ItemType.Potion => nameof(ItemType.Potion),
                ItemType.Chest => nameof(ItemType.Chest),
                ItemType.Amulet => nameof(ItemType.Amulet),
                ItemType.Recipe => nameof(ItemType.Recipe),
                _ => "none",
            };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static IEmote GetEmoji(this Language target)
        {
            return target switch
            {
                Language.Eng => new Emoji("🇺🇸"),
                Language.Ru => new Emoji("🇷🇺"),
                _ => string.Empty
            };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static string GetName(this Language target)
        {
            return target switch
            {
                Language.Eng => "eng",
                Language.Ru => "ru",
                _ => string.Empty
            };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static IEnumerable<TEnum> GetFlags<TEnum>(this TEnum input)
            where TEnum: Enum
        {
            foreach (TEnum value in Enum.GetValues(input.GetType()))
                if (input.HasFlag(value))
                    yield return value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static IEmote? GetBadgeEmoji(this UserBadges target)
        {
            return target switch
            {
                UserBadges.None => null,
                UserBadges.BugHunter => EmojiService.Get("badgeBugHunter"),
                _ => null
            };
        }
    }
}
