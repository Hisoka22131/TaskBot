using TaskBot.Services.Commands.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TaskBot.Services.Commands;

public class SendMessageCommand : ICommand
{
    public async Task Execute(ITelegramBotClient telegramBotClient, Update update, string message) =>
        await telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, message);
}