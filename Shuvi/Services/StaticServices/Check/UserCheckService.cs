using Shuvi.Classes.Data.Settings;

namespace Shuvi.Services.StaticServices.Check
{
    public static class UserCheckService
    {
        private static AdminsData _admins = new();

        public static void Init(AdminsData admins)
        {
            _admins = admins;
        }
        public static bool isAdmin(ulong userId)
        {
            return _admins.AdminIds.Contains(userId);
        }
    }
}
