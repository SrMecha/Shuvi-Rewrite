using Discord;
using Shuvi.Enums.Characteristic;
using Shuvi.Enums.Item;
using Shuvi.Enums.Localization;
using Shuvi.Enums.Magic;
using Shuvi.Enums.Money;
using Shuvi.Enums.Rating;
using Shuvi.Enums.Requirements;
using Shuvi.Enums.Top;
using Shuvi.Enums.User;
using Shuvi.Services.StaticServices.Emoji;
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
        public static string GetName(this BaseRequirement target)
        {
            return target switch
            {
                BaseRequirement.Strength => "Strength",
                BaseRequirement.Agility => "Agility",
                BaseRequirement.Luck => "Luck",
                BaseRequirement.Intellect => "Intellect",
                BaseRequirement.Endurance => "Endurance",
                BaseRequirement.Rank => "Rank",
                _ => string.Empty
            };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static string GetName(this DynamicCharacteristic target)
        {
            return target switch
            {
                DynamicCharacteristic.Health => "Health",
                DynamicCharacteristic.Energy => "Energy",
                DynamicCharacteristic.Mana => "Mana",
                _ => string.Empty
            };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static string GetName(this MagicType target)
        {
            return target switch
            {
                MagicType.None => "None",
                MagicType.Fire => "Fire",
                MagicType.Water => "Water",
                MagicType.Wind => "Wind",
                MagicType.Earth => "Earth",
                MagicType.Losen => "Losen",
                MagicType.Ice => "Ice",
                _ => "None"
            };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static string GetName(this StaticCharacteristic target)
        {
            return target switch
            {
                StaticCharacteristic.Strength => "Strength",
                StaticCharacteristic.Agility => "Agility",
                StaticCharacteristic.Luck => "Luck",
                StaticCharacteristic.Intellect => "Intellect",
                StaticCharacteristic.Endurance => "Endurance",
                _ => string.Empty
            };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static string GetName(this MoneyType target)
        {
            return target switch
            {
                MoneyType.Gold => "Gold",
                MoneyType.Dispoints => "Dispoints",
                _ => string.Empty
            };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static string GetName(this UserRace target)
        {
            return target switch
            {
                UserRace.ExMachina => "ExMachina",
                UserRace.Beastmen => "Beastmen",
                _ => "None"
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
        public static string GetName(this UserSubrace target)
        {
            return target switch
            {
                UserSubrace.NoSubrace => "None",
                UserSubrace.Werewolf => "Werewolf",
                _ => "None"
            };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static string GetName(this UserProfession target)
        {
            return target switch
            {
                UserProfession.NoProfession => "None",
                UserProfession.Prufer => "Prufer",
                UserProfession.Kampfer => "Kampfer",
                UserProfession.Hunter => "Hunter",
                _ => "None"
            };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static string GetName(this ItemType target)
        {
            return target switch
            {
                ItemType.Simple => "Simple",
                ItemType.Weapon => "Weapon",
                ItemType.Helmet => "Helmet",
                ItemType.Armor => "Armor",
                ItemType.Leggings => "Leggings",
                ItemType.Boots => "Boots",
                ItemType.Potion => "Potion",
                ItemType.Chest => "Chest",
                ItemType.Amulet => "Amulet",
                ItemType.Recipe => "Recipe",
                _ => "None",
            };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static string GetName(this UserTopType target)
        {
            return target switch
            {
                UserTopType.Rating => "Rating",
                UserTopType.Gold => "Gold",
                _ => "None",
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
                _ => "None",
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
                Language.Eng => "Eng",
                Language.Ru => "Ru",
                _ => string.Empty
            };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static IEnumerable<TEnum> GetFlags<TEnum>(this TEnum input)
            where TEnum : Enum
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
                UserBadges.AlphaTester => EmojiService.Get("BadgeAlphaTester"),
                UserBadges.BetaTester => EmojiService.Get(""),
                UserBadges.BugHunter => EmojiService.Get("BadgeBugHunter"),
                _ => null
            };
        }
    }
}
