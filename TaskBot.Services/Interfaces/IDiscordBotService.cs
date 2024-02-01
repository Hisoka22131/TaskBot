using Discord.WebSocket;

namespace TaskBot.Services.Interfaces;

public interface IDiscordBotService
{
    Task HandleMessage(SocketMessage arg);
}