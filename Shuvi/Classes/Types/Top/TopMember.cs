using Discord;
using Shuvi.Interfaces.Top;
using Shuvi.Interfaces.User;
using Shuvi.Services.StaticServices.Database;

namespace Shuvi.Classes.Types.Top
{
    public class TopMember : ITopMember
    {
        public ulong Id { get; init; }
        public int Place { get; init; }
        public string Name { get; init; }
        public int Amount { get; init; }

        public TopMember()
        {
            Id = 0;
            Name = "UserNotProvided";
            Amount = -1;
            Place = -1;
        }

        public TopMember(IUser user, int amount, int place)
        {
            Id = user.Id;
            Name = user.Username;
            Amount = amount;
            Place = place;
        }

        public async Task<IDatabaseUser> GetDatabaseUser()
        {
            return await UserDatabase.GetUser(Id);
        }
    }
}
