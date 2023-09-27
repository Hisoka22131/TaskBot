using TaskBot.Services.Commands.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using File = System.IO.File;

namespace TaskBot.Services.Commands;

public class SendTaskFileCommand : ICommand
{
    public SendTaskFileCommand(ICollection<Library.Models.Task> tasks)
    {
        Tasks = tasks;
    }

    private ICollection<Library.Models.Task> Tasks;

    public async Task Execute(ITelegramBotClient telegramBotClient, Update update, string message)
    {
        var tempFileName = Path.GetTempFileName();

        try
        {
            foreach (var task in Tasks)
            {
                await File.AppendAllTextAsync(tempFileName,
                    $"Задача: # {task.Number}\nОписание: {task.Description}\nРезультат: {task.Result}\n\n");
            }

            await using var stream = File.OpenRead(tempFileName);
            var inputOnlineFile = new InputFileStream(stream, "задачи.txt");
            
            await telegramBotClient.SendDocumentAsync(update.Message!.Chat.Id, inputOnlineFile, caption: message);
        }
        catch (Exception ex)
        {
            await  telegramBotClient.SendTextMessageAsync(update.Message!.Chat.Id, $"Произошла ошибка: {ex.Message}");
            throw;
        }
    }
}