using Shuvi.Interfaces.Pet;
using Shuvi.Interfaces.User;
using Shuvi.Services.StaticServices.Database;

namespace Shuvi.Classes.Types.Pet
{
    public class PetMasterInfo : IPetMasterInfo
    {
        public ulong MasterId { get; private set; }

        public PetMasterInfo(ulong masterId)
        {
            MasterId = masterId;
        }
        public async Task<IDatabaseUser> GetMaster()
        {
            return (await UserDatabase.TryGetUser(MasterId))!;
        }
    }
}
