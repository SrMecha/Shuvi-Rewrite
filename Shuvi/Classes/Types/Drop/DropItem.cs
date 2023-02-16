using MongoDB.Bson;
using Shuvi.Classes.Data.Drop;
using Shuvi.Interfaces.Drop;

namespace Shuvi.Classes.Types.Drop
{
    public sealed class DropItem : IDropItem
    {
        public ObjectId Id { get; private set; }
        public float Chance { get; private set; }
        public int Max { get; private set; }
        public int Min { get; private set; }

        public DropItem(ObjectId id, float chance, int max, int min)
        {
            Id = id;
            Chance = chance;
            Max = max;
            Min = min;
        }
        public DropItem(DropItemData data)
        {
            Id = data.Id;
            Chance = data.Chance;
            Max = data.Max;
            Min = data.Min;
        }
    }
}
