using Discord;

namespace Shuvi.Classes.Extensions
{
    public static class UserMessageExt
    {
        public static async Task RemoveButtonsAsync(this IUserMessage message)
        {
            await message.ModifyAsync(msg => { msg.Components = new ComponentBuilder().Build(); });
        }
    }
}
