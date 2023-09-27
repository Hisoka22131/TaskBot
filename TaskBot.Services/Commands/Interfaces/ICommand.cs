using Telegram.Bot;
using Telegram.Bot.Types;

namespace TaskBot.Services.Commands.Interfaces;

public interface ICommand
{
    Task Execute(ITelegramBotClient telegramBotClient, Update update, string message);
}