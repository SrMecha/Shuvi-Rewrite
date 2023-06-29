using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using Shuvi.Classes.Data.User;
using Shuvi.Classes.Extensions;
using Shuvi.Classes.Factories.CustomEmbed;
using Shuvi.Enums.Image;
using Shuvi.Enums.Localization;
using Shuvi.Services.StaticServices.Database;
using Shuvi.Services.StaticServices.Emoji;

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
                [Summary("userId")] ulong userId,
                [Summary("id")] string id,
                [Summary("amount")] int amount
                )
        {
            var dbUser = await UserDatabase.TryGetUser(userId);
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
                userId,
                new UpdateDefinitionBuilder<UserData>().Set(x => x.Inventory, dbUser.Inventory.GetItemsDictionary()));
            var embed = EmbedFactory.CreateInfoEmbed($"Вы успешно выдали {item.Info.GetName(Language.Ru)} x{amount} пользователю <@{userId}>.");
            await ReplyAsync(embed: embed);
        }

        [Command("Items", true)]
        public async Task ItemsCommandAsync()
        {
            var currentPage = 0;
            var maxPage = ItemDatabase.Items.Count / 10;
            var arrow = 0;
            SocketMessageComponent? interaction = null;
            IUserMessage? message = null;
            while (true)
            {
                var item = ItemDatabase.Items[currentPage * 10 + arrow];
                var embed = EmbedFactory.CreateUserEmbed(Context.User)
                    .WithDescription($"ID: {item.Id}\n\n{GetItemsString(currentPage, arrow)}")
                    .WithFooter($"Страница {currentPage + 1}/{maxPage + 1}")
                    .Build();
                var components = new ComponentBuilder()
                    .WithSelectMenu("select", GetItemsOptions(currentPage, arrow),
                    "Выберите предмет", row: 0)
                    .WithButton("<", "<", ButtonStyle.Primary, disabled: currentPage < 1, row: 1)
                    .WithButton("Выйти", "exit", ButtonStyle.Danger, row: 1)
                    .WithButton(">", ">", ButtonStyle.Primary, disabled: currentPage >= maxPage, row: 1)
                    .Build();
                if (message is null)
                    message = await Context.Message.ReplyAsync(embed: embed, components: components);
                else
                    await message.ModifyAsync(msg => { msg.Embed = embed; msg.Components = components; });
                if (interaction is not null)
                    await interaction.TryDeferAsync();
                interaction = await _client.WaitForButtonInteraction(message, Context.User.Id);
                if (interaction is null)
                {
                    await message.DeleteAsync();
                    return;
                }
                switch (interaction.Data.CustomId)
                {
                    case "select":
                        arrow = int.Parse(interaction.Data.Values.First());
                        break;
                    case "<":
                        arrow = 0;
                        currentPage--;
                        break;
                    case ">":
                        arrow = 0;
                        currentPage++;
                        break;
                    case "exit":
                        await message.DeleteAsync();
                        return;

                }
            }
        }

        private static string GetItemsString(int page, int arrow)
        {
            var row = 0;
            var result = new List<string>();
            for (int i = page * 10; i < ItemDatabase.Items.Count && i < page * 10 + 10; i++)
            {
                var item = ItemDatabase.Items[i];
                result.Add(row == arrow ? $"{EmojiService.Get("choosePoint")}{item.Info.GetName(Language.Ru)}" : item.Info.GetName(Language.Ru));
                row++;
            }
            return result.Count < 1 ? "Пусто" : string.Join("\n", result);
        }

        private static List<SelectMenuOptionBuilder> GetItemsOptions(int page, int arrow)
        {
            var result = new List<SelectMenuOptionBuilder>();
            var row = 0;
            for (int i = page * 10; i < ItemDatabase.Items.Count && i < page * 10 + 10; i++)
            {
                var item = ItemDatabase.Items[i];
                result.Add(new(item.Info.GetName(Language.Ru), row.ToString()));
                row++;
            }
            if (result.Count < 1)
                result.Add(new("Пусто", "empty"));
            return result;
        }
    }
}
