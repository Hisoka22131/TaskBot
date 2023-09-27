using Newtonsoft.Json;
using TaskBot.Repository.Repo.Abstract;
using TaskBot.Repository.UoF;
using TaskBot.Services.Commands;
using TaskBot.Services.Commands.Interfaces;
using TaskBot.Services.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TaskBot.Services.Services;

public class BotService : IBotService
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IUserService _userService;

    public BotService(ITelegramBotClient telegramBotClient, IUserService userService)
    {
        _telegramBotClient = telegramBotClient;
        _userService = userService;
    }


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
            case not null when command.StartsWith("/start"):
            {
                var invoker = new CommandInvoker(new SendMessageCommand());
                await invoker.ExecuteCommand(_telegramBotClient, update, "Привет, " + update.Message.Chat.Username);

                break;
            }
            case not null when command.StartsWith("/gettasks"):
            {
                var tasks = await _userService.GetUserTasks(update.Message.Chat.Id);

                var invoker = new CommandInvoker(new SendMessageCommand());
                await invoker.ExecuteCommand(_telegramBotClient, update, tasks);

                break;
            }
        }
    }
}