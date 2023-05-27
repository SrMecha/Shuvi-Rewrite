using Discord;
using Discord.WebSocket;
using Shuvi.Classes.Data.User;
using Shuvi.Classes.Types.Top;
using Shuvi.Enums.Top;
using Shuvi.Interfaces.Top;
using Shuvi.Services.StaticServices.Database;

namespace Shuvi.Services.StaticServices.Top
{
    public static class TopService
    {
        public static Dictionary<UserTopType, List<ITopMember>> _userTop = new();

        public static Dictionary<UserTopType, long> _lastUpdate = new();

        public static async Task UpdateTop(DiscordShardedClient client, UserTopType type, List<UserData> users)
        {
            var top = new List<ITopMember>();
            for (int i = 0; i < users.Count; i++)
            {
                var userData = users[i];
                var socketUser = client.GetUser(userData.Id);
                if (socketUser is null)
                    top.Add(new TopMember(await client.Rest.GetUserAsync(userData.Id), GetAmount(type, userData), i + 1));
                else
                    top.Add(new TopMember(socketUser, GetAmount(type, userData), i + 1));
                await Task.Delay(1000);
            }
            if (_userTop.ContainsKey(type))
            {
                _userTop[type] = top;
                _lastUpdate[type] = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
            }
            else
            {
                _userTop.Add(type, top);
                _lastUpdate.Add(type, ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds());
            }
        }

        private static int GetAmount(UserTopType type, UserData userData)
        {
            return type switch
            {
                UserTopType.Rating => userData.Rating,
                UserTopType.Gold => userData.Gold,
                _ => -1
            };
        }

        public static void StartUpdateTop(DiscordShardedClient client)
        {
            _ = Task.Run(async () => {
                while (true)
                {
                    await Task.Delay(new TimeSpan(0, 0, 5));
                    if (client.LoginState == LoginState.LoggedIn)
                        break;
                }
                while (true)
                {
                    await UpdateTop(client, UserTopType.Rating, await UserDatabase.GetTopUsers(x => x.Rating, 100));
                    await UpdateTop(client, UserTopType.Gold, await UserDatabase.GetTopUsers(x => x.Gold, 100));
                    await Task.Delay(new TimeSpan(0, 5, 0));
                }
            });
        }

        public static IEnumerable<ITopMember> GetTopMembersInPage(UserTopType type, int page)
        {
            var top = _userTop.GetValueOrDefault(type, new());
            for (int i = page * 10; i < top.Count && i < page * 10 + 10; i++)
                yield return top[i];
        }

        public static bool IsDataSet(UserTopType type)
        {
            return _userTop.ContainsKey(type);
        }

        public static long GetUpdateTime(UserTopType type)
        {
            if (_lastUpdate.ContainsKey(type))
                return _lastUpdate[type];
            return 0;
        }

        public static int GetTotalPages(UserTopType type)
        {
            return (_userTop.GetValueOrDefault(type, new()).Count) / 10 < 1 ? 1 : (_userTop.GetValueOrDefault(type, new()).Count + 9) / 10;
        }
    } 
}
