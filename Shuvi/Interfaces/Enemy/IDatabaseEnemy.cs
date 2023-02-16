using MongoDB.Bson;
using Shuvi.Interfaces.Localization;

namespace Shuvi.Interfaces.Enemy
{
    public interface IDatabaseEnemy
    {
        public ObjectId Id { get; }
        public ILocalizedInfo Info { get; }

    }
}
