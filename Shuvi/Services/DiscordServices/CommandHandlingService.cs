﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Shuvi.Services.StaticServices.Check;
using System.Reflection;

namespace Shuvi.Services.DiscordServices
{
    public class CommandHandlingService
    {
        private readonly CommandService _commands;
        private readonly DiscordShardedClient _discord;
        private readonly IServiceProvider _services;

        public CommandHandlingService(IServiceProvider services)
        {
            _commands = services.GetRequiredService<CommandService>();
            _discord = services.GetRequiredService<DiscordShardedClient>();
            _services = services;

            // Hook CommandExecuted to handle post-command-execution logic.
            _commands.CommandExecuted += CommandExecutedAsync;
            // Hook MessageReceived so we can process each message to see
            // if it qualifies as a command.
            _discord.MessageReceived += MessageReceivedAsync;
        }

        public async Task InitializeAsync()
        {
            // Register modules that are public and inherit ModuleBase<T>.
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        public Task MessageReceivedAsync(SocketMessage rawMessage)
        {
            _ = Task.Run(async () => {
                // Ignore system messages, or messages from other bots
                if (!(rawMessage is SocketUserMessage message))
                    return;
                if (message.Source != MessageSource.User)
                    return;

                // This value holds the offset where the prefix ends
                var argPos = 0;
                // Perform prefix check. You may want to replace this with
                // (!message.HasCharPrefix('!', ref argPos))
                // for a more traditional command format like !help.
                if (!message.HasCharPrefix('!', ref argPos))
                    return;

                var context = new ShardedCommandContext(_discord, message);
                // Perform the execution of the command. In this method,
                // the command service will perform precondition and parsing check
                // then execute the command if one is matched.
                if (UserCheckService.isAdmin(context.User.Id))
                    await _commands.ExecuteAsync(context, argPos, _services);
                // Note that normally a result will be returned by this format, but here
                // we will handle the result in CommandExecutedAsync,
            });
            return Task.CompletedTask;
        }

        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            // command is unspecified when there was a search failure (command not found); we don't care about these errors
            if (!command.IsSpecified)
                return;

            // the command was successful, we don't care about this result, unless we want to log that a command succeeded.
            if (result.IsSuccess)
                return;

            // the command failed, let's notify the user that something happened.
            if (result is ExecuteResult execResult)
                await context.Channel.SendMessageAsync($"error: {execResult.Exception}");
            else
                await context.Channel.SendMessageAsync($"error: {result.ErrorReason}");
        }
    }
}
