using Telegram.Bot;

namespace TaskBot.Services.Interfaces;

public interface IBotService
{
    Task<object> Handle(object update);
}