namespace Shuvi.Classes.Data.Drop
{
    public sealed class ChestDropData
    {
        public MoneyDropData Money { get; set; } = new();
        public List<DropItemData> Items { get; set; } = new();
    }
}
