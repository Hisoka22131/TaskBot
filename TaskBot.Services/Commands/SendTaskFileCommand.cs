using TaskBot.Services.Commands.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using File = System.IO.File;

namespace TaskBot.Services.Commands;

public class SendTaskFileCommand : ICommand
{
    public SendTaskFileCommand(ICollection<Library.Models.Task> tasks)
    {
        _tasks = tasks;
    }

    private readonly ICollection<Library.Models.Task> _tasks;

    public async Task Execute(ITelegramBotClient telegramBotClient, Update update, string message)
    {
        var tempFileName = Path.GetTempFileName();

        try
        {
            foreach (var task in _tasks)
            {
                await File.AppendAllTextAsync(tempFileName,
                    $"Задача: # {task.Number} {task.TaskClose}\n" + 
                    $"Описание: {task.Description}\n" +
                    $"Результат: {task.Result}\n\n");
            }

            await using var stream = File.OpenRead(tempFileName);
            var inputOnlineFile = new InputFileStream(stream, "задачи.txt");
            
            await telegramBotClient.SendDocumentAsync(update.Message!.Chat.Id, inputOnlineFile, caption: message);
        }
        catch (Exception ex)
        {
            await telegramBotClient.SendTextMessageAsync(update.Message!.Chat.Id, $"Произошла ошибка: {ex.Message}");
            throw;
        }
    }
}