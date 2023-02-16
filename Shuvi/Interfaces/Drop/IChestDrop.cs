namespace Shuvi.Interfaces.Drop
{
    public interface IChestDrop
    {
        public IDropMoney Money { get; }
        public List<IDropItem> Items { get; }
    }
}
