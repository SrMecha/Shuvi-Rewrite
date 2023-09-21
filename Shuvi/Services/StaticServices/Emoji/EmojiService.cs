using Discord;

namespace Shuvi.Services.StaticServices.Emoji
{
    public static class EmojiService
    {
        public static readonly IDictionary<string, IEmote> _emotes = new Dictionary<string, IEmote>();
        private static readonly IEmote _notFound = Emote.Parse("<:404:1036314484790280232>");

        public static void Init()
        {
            IDictionary<string, string> emotesBase = new Dictionary<string, string>
            {
                { "_", "<:404:1036314484790280232>" },
                { "GoodMark", "<:checked:1031196185794449470>" },
                { "BadMark", "<:canceled:1031196187870629898>" },
                { "Gold", "<:gold_coins:1066321458441236562>" },
                { "Dispoints", "<:dispoint:1015999481034068048>" },
                { "EnergyEmpty", "<:energy_empty:1030943969863024730>" },
                { "EnergyFull", "<:energy_full:1030943971532349491>" },
                { "HealthEmpty", "<:health_empty:1030948347529412628>" },
                { "HealthFull", "<:health_full:1030948349022588958>" },
                { "ChoosePoint", "<a:right_point:1031199297301131285>" },
                { "MagicFull", "<:magic_full:1050772671002071160>" },
                { "MagicEmpty", "<:magic_empty:1050772733828530218>" },
                { "LineMiddle", "<:LineMiddle:1062300869808369726>" },
                { "LineEnd", "<:LineEnd:1062300873113468938>" },
                { "BadgeBugHunter", "<:bug_hunter:1112409224861982850>" },
                { "BadgeAlphaTester", "<:alpha_tester:1112408736057806868>" },
                { "GuildEnter", "<:guild_enter:1112407946899492934>" },
                { "GuildLeave", "<:guild_leave:1112407949709672469>" },
                { "NewPlayer", "<:new_player:1112419534503157901>" },
                { "PlayerDead", "<:player_dead:1112407952092053595>" },
                { "RankUp", "<:rank_up:1112419536143130677>" }
            };
            foreach (var (emoteKey, emoteCode) in emotesBase)
                if (Emote.TryParse(emoteCode, out var emote))
                    _emotes[emoteKey] = emote;
        }

        public static IEmote Get(string name)
        {
            if (_emotes.TryGetValue(name, out var emote))
                return emote;
            return _notFound;
        }
    }
}
