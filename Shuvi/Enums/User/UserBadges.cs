namespace Shuvi.Enums.User
{
    [Flags]
    public enum UserBadges
    {
        None = 0,
        AlphaTester = 1 << 0,
        BetaTester = 1 << 1,
        BugHunter = 1 << 2
    }
}
