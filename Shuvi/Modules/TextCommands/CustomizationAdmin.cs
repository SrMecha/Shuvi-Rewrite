using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using Shuvi.Classes.Data.User;
using Shuvi.Classes.Extensions;
using Shuvi.Classes.Factories.CustomEmbed;
using Shuvi.Classes.Types.Customization;
using Shuvi.Enums.Image;
using Shuvi.Enums.Localization;
using Shuvi.Enums.User;
using Shuvi.Interfaces.Customization;
using Shuvi.Interfaces.Items;
using Shuvi.Services.StaticServices.Database;
using Shuvi.Services.StaticServices.Emoji;

namespace Shuvi.Modules.TextCommands
{
    public class CustomizationAdminCommandModule : ModuleBase<SocketCommandContext>
    {
        private readonly DiscordShardedClient _client;
        private readonly CommandService _commands;

        public CustomizationAdminCommandModule(IServiceProvider provider)
        {
            _client = provider.GetRequiredService<DiscordShardedClient>();
            _commands = provider.GetRequiredService<CommandService>();
        }

        [Command("GiveImage", true)]
        public async Task GiveCustomizationCommandAsync(
            [Summary("userId")] ulong userId,
            [Summary("imageId")] string imageId
            )
        {
            var image = ImageDatabase.GetImage(new ObjectId(imageId));
            var dbUser = await UserDatabase.TryGetUser(userId);
            if (dbUser is null)
            {
                await ReplyAsync(embed: EmbedFactory.CreateErrorEmbed("Пользователь еще не создал аккаунт.", Language.Ru));
                return;
            }
            dbUser.Customization.AddImage(image);
            await UserDatabase.UpdateUser(
                userId,
                new UpdateDefinitionBuilder<UserData>().Set(x => x.Images, dbUser.Customization.GetImagesCache()));
            var embed = EmbedFactory.CreateInfoEmbed($"Вы выдали пользовутелю <@{userId}> картинку **{image.Info.GetName(Language.Ru)}**");
            await ReplyAsync(embed: embed);
        }

        [Command("RemoveImage", true)]
        public async Task RemoveCustomizationCommandAsync(
            [Summary("userId")] ulong userId,
            [Summary("imageId")] string imageId
            )
        {
            var image = ImageDatabase.GetImage(new ObjectId(imageId));
            var dbUser = await UserDatabase.TryGetUser(userId);
            if (dbUser is null)
            {
                await ReplyAsync(embed: EmbedFactory.CreateErrorEmbed("Пользователь еще не создал аккаунт.", Language.Ru));
                return;
            }
            dbUser.Customization.RemoveImage(image);
            await UserDatabase.UpdateUser(
                userId,
                new UpdateDefinitionBuilder<UserData>().Set(x => x.Images, dbUser.Customization.GetImagesCache()));
            var embed = EmbedFactory.CreateInfoEmbed($"Вы забрали у пользователя <@{userId}> картинку **{image.Info.GetName(Language.Ru)}**");
            await ReplyAsync(embed: embed);
        }

        [Command("Images", true)]
        public async Task CustomizationCommandAsync() 
        {
            var currentPage = 0;
            var maxPage = ImageDatabase.Images.Count / 10;
            var arrow = 0;
            SocketMessageComponent? interaction = null;
            IUserMessage? message = null;
            while (true)
            {
                var image = ImageDatabase.Images[currentPage * 10 + arrow];
                var embed = EmbedFactory.CreateEmbed()
                    .WithDescription($"ID: {image.Id}\n\n{GetCustomizationsString(currentPage, arrow)}")
                    .WithThumbnailUrl(image.Type == ImageType.Avatar ? image.URL : string.Empty)
                    .WithImageUrl(image.Type == ImageType.Banner ? image.URL : string.Empty)
                    .WithFooter($"Страница {currentPage + 1}/{maxPage + 1}")
                    .Build();
                var components = new ComponentBuilder()
                    .WithSelectMenu("select", GetCustomizationOptions(currentPage, arrow),
                    "Выберите картинку", row: 0)
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

        private static string GetCustomizationsString(int page, int arrow)
        {
            var row = 0;
            var result = new List<string>();
            for (int i = page * 10; i < ImageDatabase.Images.Count && i < page * 10 + 10; i++)
            {
                var image = ImageDatabase.Images[i];
                result.Add(row == arrow ? $"{EmojiService.Get("choosePoint")}{image.Info.GetName(Language.Ru)}" : image.Info.GetName(Language.Ru));
                row++;
            }
            return result.Count < 1 ? "Пусто" : string.Join("\n", result);
        }

        private static List<SelectMenuOptionBuilder> GetCustomizationOptions(int page, int arrow)
        {
            var result = new List<SelectMenuOptionBuilder>();
            var row = 0;
            for (int i = page * 10; i < ImageDatabase.Images.Count && i < page * 10 + 10; i++)
            {
                var image = ImageDatabase.Images[i];
                result.Add(new(image.Info.GetName(Language.Ru), row.ToString()));
                row++;
            }
            if (result.Count < 1)
                result.Add(new("Пусто", "empty"));
            return result;
        }
    }
}
