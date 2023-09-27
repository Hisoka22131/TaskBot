using TaskBot.Services.Commands.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TaskBot.Services.Commands;

public class CommandInvoker
{
    private readonly ICommand _command;

    public CommandInvoker(ICommand command)
    {
        _command = command;
    }

    public async Task ExecuteCommand(ITelegramBotClient telegramBotClient, Update update, string message) =>
        await _command.Execute(telegramBotClient, update, message);
}