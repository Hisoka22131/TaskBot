namespace TaskBot.Services.Interfaces;

public interface ITelegramBotService
{
    Task<object> HandleMessage(object update);
}