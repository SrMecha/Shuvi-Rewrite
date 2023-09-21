using Shuvi.Classes.Data.Requirements;
using Shuvi.Classes.Extensions;
using Shuvi.Classes.Types.Status;
using Shuvi.Enums.Localization;
using Shuvi.Enums.Rating;
using Shuvi.Interfaces.Pet;
using Shuvi.Interfaces.Requirements;
using Shuvi.Interfaces.Status;
using Shuvi.Interfaces.User;
using Shuvi.Services.StaticServices.Emoji;
using Shuvi.Services.StaticServices.Localization;

namespace Shuvi.Classes.Types.Requirements
{
    public class BaseRequirements : IBaseRequirements
    {
        public int Strength { get; init; } = 0;
        public int Agility { get; init; } = 0;
        public int Luck { get; init; } = 0;
        public int Intellect { get; init; } = 0;
        public int Endurance { get; init; } = 0;
        public int Rank { get; init; } = 0;

        public BaseRequirements() { }

        public BaseRequirements(RequirementsData data)
        {
            Strength = data.Strength;
            Agility = data.Agility;
            Luck = data.Luck;
            Intellect = data.Intellect;
            Endurance = data.Endurance;
            Rank = data.Rank;
        }

        public IRequirementResult GetRequirementsInfo(Language lang)
        {
            var result = new List<string>();
            foreach (var (requirement, amount) in GetRequirements())
            {
                if (amount == 0)
                    continue;
                result.Add($"{LocalizationService.Get("names").Get(lang).Get(requirement)} {FormatRequirement(requirement, amount)}+");
            }
            if (result.Count < 1)
                return new RequirementResult(false, LocalizationService.Get("names").Get(lang).Get("NotHave"));
            return new RequirementResult(false, string.Join("\n", result));
        }
        public IRequirementResult GetRequirementsInfo(Language lang, IDatabaseUser user)
        {
            var result = new List<string>();
            var isMeetAllRequirements = true;
            foreach (var (requirement, amount) in GetRequirements())
            {
                var isMeetRequirement = true;
                if (amount == 0)
                    continue;
                if (!IsMeetRequirement(requirement, amount, user))
                {
                    isMeetRequirement = false;
                    isMeetAllRequirements = false;
                }
                result.Add($"{(isMeetRequirement ? EmojiService.Get("GoodMark") : EmojiService.Get("badMark"))} " +
                    $"{LocalizationService.Get("names").Get(lang).Get(requirement)} {FormatRequirement(requirement, amount)}+");
            }
            if (result.Count < 1)
                return new RequirementResult(isMeetAllRequirements, LocalizationService.Get("names").Get(lang).Get("NotHave"));
            return new RequirementResult(isMeetAllRequirements, string.Join("\n", result));
        }
        public IRequirementResult GetRequirementsInfo(Language lang, IDatabasePet pet)
        {
            var result = new List<string>();
            var isMeetAllRequirements = true;
            foreach (var (requirement, amount) in GetRequirements())
            {
                var isMeetRequirement = true;
                if (amount == 0)
                    continue;
                if (!IsMeetRequirement(requirement, amount, pet))
                {
                    isMeetRequirement = false;
                    isMeetAllRequirements = false;
                }
                result.Add($"{(isMeetRequirement ? EmojiService.Get("GoodMark") : EmojiService.Get("badMark"))} " +
                    $"{LocalizationService.Get("names").Get(lang).Get(requirement)} {FormatRequirement(requirement, amount)}+");
            }
            if (result.Count < 1)
                return new RequirementResult(isMeetAllRequirements, LocalizationService.Get("names").Get(lang).Get("NotHave"));
            return new RequirementResult(isMeetAllRequirements, string.Join("\n", result));
        }
        public bool IsMeetRequirement(string requirement, int amount, IDatabaseUser user)
        {
            return requirement switch
            {
                "Strength" => user.Characteristics.Strength >= amount,
                "Agility" => user.Characteristics.Agility >= amount,
                "Luck" => user.Characteristics.Luck >= amount,
                "Intellect" => user.Characteristics.Intellect >= amount,
                "Endurance" => user.Characteristics.Endurance >= amount,
                "Rank" => (int)user.Rating.Rank >= amount,
                _ => false
            };
        }
        public bool IsMeetRequirement(string requirement, int amount, IDatabasePet pet)
        {
            return requirement switch
            {
                "Strength" => pet.Characteristics.Strength >= amount,
                "Agility" => pet.Characteristics.Agility >= amount,
                "Luck" => pet.Characteristics.Luck >= amount,
                "Intellect" => pet.Characteristics.Intellect >= amount,
                "Endurance" => pet.Characteristics.Endurance >= amount,
                "Rank" => (int)pet.Rank >= amount,
                _ => false
            };
        }

        public IEnumerable<(string, int)> GetRequirements()
        {
            yield return ("Strength", Strength);
            yield return ("Agility", Agility);
            yield return ("Luck", Luck);
            yield return ("Intellect", Intellect);
            yield return ("Endurance", Endurance);
            yield return ("Rank", Rank);
        }

        private static string FormatRequirement(string requirement, int amount)
        {
            return requirement switch
            {
                "Rank" => ((Rank)amount).GetName(),
                _ => amount.ToString()
            };
        }
    }
}
