using MongoDB.Bson;
using Shuvi.Interfaces.User;

namespace Shuvi.Interfaces.Pet
{
    public interface IPetMasterInfo
    {
        public ulong MasterId { get; }
        public Task<IDatabaseUser> GetMaster();
    }
}
