using Shuvi.Classes.Data.Settings;
using Shuvi.Enums.Check;

namespace Shuvi.Services.StaticServices.Check
{
    public static class UserCheckService
    {
        private static AdminsData _admins = new();
        private static readonly Dictionary<TrackedCommand, List<ulong>> _trackedCommands = new();

        public static void Init(AdminsData admins)
        {
            _admins = admins;
        }
        public static bool isAdmin(ulong userId)
        {
            return _admins.AdminIds.Contains(userId);
        }
        public static bool IsUseCommand(TrackedCommand command, ulong id)
        {
            return _trackedCommands.GetValueOrDefault(command, new()).Contains(id);
        }
        public static void AddUserToCommand(TrackedCommand command, ulong id)
        {
            if (_trackedCommands.ContainsKey(command))
                _trackedCommands[command].Add(id);
            else
                _trackedCommands[command] = new() { id };
        }
        public static void RemoveUserFromCommand(TrackedCommand command, ulong id)
        {
            if (_trackedCommands.ContainsKey(command))
                _trackedCommands[command].Remove(id);
        }
    }
}
