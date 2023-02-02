using MongoDB.Bson;
using Shuvi.Classes.Data.Item;
using Shuvi.Enums.Money;

namespace Shuvi.Classes.Data.Drop
{
    public sealed class ChestDropData
    {
        public Dictionary<MoneyType, MoneyDropData> MoneyDrop { get; set; } = new();
        public int ItemsMin { get; set; } = 0;
        public int ItemsMax { get; set; } = 0;
        public Dictionary<ObjectId, int> Items { get; set; } = new();
    }
}
