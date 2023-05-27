using Shuvi.Interfaces.User;

namespace Shuvi.Interfaces.Top
{
    public interface ITopMember
    {
        public ulong Id { get; init; }
        public int Place { get; init; }
        public string Name { get; init; }
        public int Amount { get; init; }
        public Task<IDatabaseUser> GetDatabaseUser();
    }
}
