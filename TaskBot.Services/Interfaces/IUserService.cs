using Telegram.Bot.Types;

namespace TaskBot.Services.Interfaces;

public interface IUserService
{ 
    Task CheckUser(Update update);
    Task<string?> GetUserTasks(long userChatId);
}