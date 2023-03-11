using Shuvi.Interfaces.Inventory;
using Shuvi.Interfaces.User;
using System.Runtime.CompilerServices;

namespace Shuvi.Classes.Extensions
{
    public static class DatabaseUserExt
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void AddDrop(this IDatabaseUser user, IDropInventory drop)
        {
            user.Inventory.AddItems(drop.GetItemsDictionary());
            user.Wallet.Add(drop.GetMoneyDictionary());
        }
    }
}
