using Discord;
using Discord.WebSocket;
using Shuvi.Enums.Rating;
using Shuvi.Interfaces.User;

namespace Shuvi.Services.StaticServices.Event
{
    public static class EventManager
    {
        public static event Func<IUser, ValueTask> OnAccountCreate = (_) => default;
        public static event Func<IDatabaseUser, ValueTask> OnPlayerDie = (_) => default;
        public static event Func<IDatabaseUser, Rank, Rank, ValueTask> OnPlayerRankUp = (_, _, _) => default;
        public static event Func<SocketGuild, ValueTask> OnGuildEnter = (_) => default;
        public static event Func<SocketGuild, ValueTask> OnGuildLeave = (_) => default;

        public static void InvokeOnAccountCreate(IUser user)
        {
            Task.Run(async () =>
            {
                await OnAccountCreate.Invoke(user);
            });
        }

        public static void InvokeOnPlayerDie(IDatabaseUser dbUser)
        {
            Task.Run(async () =>
            {
                await OnPlayerDie.Invoke(dbUser);
            });
        }

        public static void InvokeOnPlayerRankUp(IDatabaseUser dbUser, Rank rankBefore, Rank rankAfter)
        {
            Task.Run(async () =>
            {
                await OnPlayerRankUp.Invoke(dbUser, rankBefore, rankAfter);
            });
        }

        public static void InvokeOnGuildEnter(SocketGuild guild)
        {
            Task.Run(async () =>
            {
                await OnGuildEnter.Invoke(guild);
            });
        }

        public static void InvokeOnGuildLeave(SocketGuild guild)
        {
            Task.Run(async () =>
            {
                await OnGuildLeave.Invoke(guild);
            });
        }
    }
}
