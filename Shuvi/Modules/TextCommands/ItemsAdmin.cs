using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using Shuvi.Classes.Data.User;
using Shuvi.Classes.Factories.CustomEmbed;
using Shuvi.Enums.Localization;
using Shuvi.Services.StaticServices.Database;

namespace Shuvi.Modules.TextCommands
{
    public class ItemsAdminCommandModule : ModuleBase<SocketCommandContext>
    {
        private readonly DiscordShardedClient _client;

        public ItemsAdminCommandModule(IServiceProvider provider)
        {
            _client = provider.GetRequiredService<DiscordShardedClient>();
        }

        [Command("GiveItem", true)]
        public async Task GiveItemCommandAsync(
                [Summary("user")] IUser paramUser,
                [Summary("id")] string id,
                [Summary("amount")] int amount
                )
        {
            var dbUser = await UserDatabase.TryGetUser(paramUser.Id);
            if (dbUser is null)
            {
                await ReplyAsync(embed: EmbedFactory.CreateErrorEmbed("Пользователь еще не создал аккаунт.", Language.Ru));
                return;
            }
            var item = ItemDatabase.GetItem(new ObjectId(id));
            if (item.Id == ObjectId.Empty)
            {
                await ReplyAsync(embed: EmbedFactory.CreateErrorEmbed("Предмет не найден.", Language.Ru));
                return;
            }
            dbUser.Inventory.AddItem(new ObjectId(id), amount);
            await UserDatabase.UpdateUser(
                paramUser.Id,
                new UpdateDefinitionBuilder<UserData>().Set(x => x.Inventory, dbUser.Inventory.GetItemsDictionary()));
            var embed = EmbedFactory.CreateInfoEmbed($"Вы успешно выдали {item.Info.GetName(Language.Ru)} x{amount} пользователю {paramUser.Username}.");
            await ReplyAsync(embed: embed);
        }
    }
}
