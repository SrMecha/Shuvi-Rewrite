using MongoDB.Bson;

namespace Shuvi.Interfaces.Pet
{
    public interface IUserPetInfo
    {
        public ObjectId? Id { get; }
        public bool HavePet { get; }
        public Task<IPet> GetPet();
    }
}
