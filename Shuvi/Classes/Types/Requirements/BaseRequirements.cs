﻿using Shuvi.Classes.Extensions;
using Shuvi.Enums.Localization;
using Shuvi.Enums.Requirements;
using Shuvi.Interfaces.Requirements;
using Shuvi.Interfaces.User;
using Shuvi.Services.StaticServices.Emoji;
using Shuvi.Services.StaticServices.Localization;

namespace Shuvi.Classes.Types.Requirements
{
    public class BaseRequirements : IBaseRequirements
    {
        protected Dictionary<BaseRequirement, int> _requirements = new();

        public BaseRequirements(Dictionary<BaseRequirement, int> requirements)
        {
            _requirements = requirements;
        }
        public BaseRequirements() { }
        public string GetRequirementsInfo(Language lang)
        {
            var result = new List<string>();
            foreach (var (requirement, amount) in _requirements)
                result.Add($"{LocalizationService.Get("names").Get(lang).Get(requirement.GetLowerName())} >{amount}");
            return string.Join("\n", result);
        }
        public string GetRequirementsInfo(Language lang, IDatabaseUser user)
        {
            var result = new List<string>();
            foreach (var (requirement, amount) in _requirements)
                result.Add($"{(IsMeetRequirement(requirement, amount, user) ? EmojiService.Get("goodMark") : EmojiService.Get("goodMark"))}" +
                    $"{LocalizationService.Get("names").Get(lang).Get(requirement.GetLowerName())} >{amount}");
            return string.Join("\n", result);
        }
        public bool IsMeetRequirements(IDatabaseUser user)
        {
            foreach (var (requirement, amount) in _requirements)
                if (!IsMeetRequirement(requirement, amount, user))
                    return false;
            return true;
        }
        public static bool IsMeetRequirement(BaseRequirement requirement, int amount, IDatabaseUser user)
        {
            return requirement switch
            {
                BaseRequirement.Strength => user.Characteristics.Strength >= amount,
                BaseRequirement.Agility => user.Characteristics.Agility >= amount,
                BaseRequirement.Luck => user.Characteristics.Luck >= amount,
                BaseRequirement.Intellect => user.Characteristics.Intellect >= amount,
                BaseRequirement.Endurance => user.Characteristics.Endurance >= amount,
                BaseRequirement.Rank => (int)user.Rating.Rank >= amount,
                _ => false
            };
        }
    }
}
