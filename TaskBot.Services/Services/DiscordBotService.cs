using System.Reflection;
using Discord.Commands;
using Discord.WebSocket;
using TaskBot.Services.Interfaces;

namespace TaskBot.Services.Services;

public class DiscordBotService : IDiscordBotService
{
    private readonly DiscordSocketClient _client;
    private readonly CommandService _commands;

    public DiscordBotService(DiscordSocketClient client, CommandService commands, IServiceProvider services)
    {
        _client = client;
        _commands = commands;
        _commands.AddModulesAsync(Assembly.GetEntryAssembly(), services);
    }

    public async Task HandleMessage(SocketMessage arg)
    {
        var message = arg as SocketUserMessage;
        if (message == null) return;

        var argPos = 0;

        if (!(message.HasCharPrefix('!', ref argPos) || 
              message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
            message.Author.IsBot)
            return;

        var context = new SocketCommandContext(_client, message);
        
        await _commands.ExecuteAsync(context: context, argPos: argPos, services: null);
    }
}