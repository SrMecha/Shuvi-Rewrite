namespace Shuvi.Enums.Premium
{
    [Flags]
    public enum PremiumAbilities
    {
        None = 0,
        ChangeColor = 1 << 0,
        InventorySort = 1 << 1
    }
}
