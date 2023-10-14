using Newtonsoft.Json;
using TaskBot.Services.Commands;
using TaskBot.Services.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TaskBot.Services.Services;

public class BotService : IBotService
{
    public BotService(ITelegramBotClient telegramBotClient, IUserService userService, ITaskService taskService)
    {
        _telegramBotClient = telegramBotClient;
        _userService = userService;
        _taskService = taskService;
    }

    #region Properties

    private readonly ITelegramBotClient _telegramBotClient;

    private readonly IUserService _userService;

    private readonly ITaskService _taskService;

    private static readonly Dictionary<long, string> CreateTaskDict = new();

    private static readonly Dictionary<long, string> CloseTaskDict = new();

    #endregion

    public async Task<object> Handle(object update)
    {
        try
        {
            var upd = JsonConvert.DeserializeObject<Update>(update.ToString() ?? string.Empty);

            switch (upd.Type)
            {
                case UpdateType.Message:
                {
                    await CheckMessageCommand(upd);
                    break;
                }
                case UpdateType.CallbackQuery:
                {
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            return "Произошла непредвиденная ошибка " + ex.Message;
        }

        return null;
    }

    private async Task CheckMessageCommand(Update update)
    {
        if (update.Message == null || string.IsNullOrWhiteSpace(update.Message.Text)) return;

        await _userService.CheckUser(update);

        var command = update.Message.Text.ToLower();

        switch (command)
        {
            case not null when command.StartsWith(Enums.Commands.Start.GetDescription()):
            {
                var invoker = new CommandInvoker();
                await invoker.SetCommand(new SendMessageCommand());

                await invoker.ExecuteCommand(_telegramBotClient, update, $"Привет, {update.Message.Chat.Username}");

                break;
            }
            case not null when command.StartsWith(Enums.Commands.GetTasks.GetDescription()):
            {
                var tasks = await _userService.GetUserTasks(update.Message.From!.Id);

                var invoker = new CommandInvoker();

                await invoker.SetCommand(tasks.Any() ? new SendTaskFileCommand(tasks) : new SendMessageCommand());

                await invoker.ExecuteCommand(_telegramBotClient, update, "Ваши задачи:");

                break;
            }
            case not null when command.StartsWith(Enums.Commands.CreateTask.GetDescription()):
            {
                CreateTaskDict.Add(update.Message.Chat.Id, Enums.Commands.CreateTask.GetDescription());

                var invoker = new CommandInvoker();
                await invoker.SetCommand(new SendMessageCommand());

                await invoker.ExecuteCommand(_telegramBotClient, update,
                    "Введите номер задачи и описание через пробел!");

                break;
            }
            case not null when command.StartsWith(Enums.Commands.CloseTask.GetDescription()):
            {
                CloseTaskDict.Add(update.Message.Chat.Id, Enums.Commands.CloseTask.GetDescription());

                var invoker = new CommandInvoker();
                await invoker.SetCommand(new SendMessageCommand());

                await invoker.ExecuteCommand(_telegramBotClient, update,"Введите номер задачи");

                break;
            }
            default:
            {
                
                await CheckCreateTaskDict(update, command!);
                await CheckCloseTaskDict(update, command!);

                break;
            }
        }
    }
    
    private async Task CheckCreateTaskDict(Update update, string command)
    {
        if (CreateTaskDict.ContainsKey(update.Message.Chat.Id) &&
            CreateTaskDict[update.Message.Chat.Id] == Enums.Commands.CreateTask.GetDescription())
        {
            var message = await _taskService.CreateTask(command!, update.Message.From!.Id);

            var invoker = new CommandInvoker();
            await invoker.SetCommand(new SendMessageCommand());

            await invoker.ExecuteCommand(_telegramBotClient, update, message);

            CreateTaskDict.Remove(update.Message.Chat.Id);
        }
    }

    private async Task CheckCloseTaskDict(Update update, string command)
    {
        if (CloseTaskDict.ContainsKey(update.Message.Chat.Id) &&
            CloseTaskDict[update.Message.Chat.Id] == Enums.Commands.CloseTask.GetDescription())
        {
            var message = await _taskService.CloseTask(command!, update.Message.From!.Id);

            var invoker = new CommandInvoker();
            await invoker.SetCommand(new SendMessageCommand());

            await invoker.ExecuteCommand(_telegramBotClient, update, message);

            CloseTaskDict.Remove(update.Message.Chat.Id);
        }
    }
}