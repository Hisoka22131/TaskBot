using TaskBot.Repository.Repo.Abstract;
using TaskBot.Repository.UoF;
using TaskBot.Services.Interfaces;
using Telegram.Bot.Types;
using Task = System.Threading.Tasks.Task;
using User = TaskBot.Library.Models.User;

namespace TaskBot.Services.Services;

public class UserService : IUserService
{
    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    private readonly IUnitOfWork _unitOfWork;
    
    private IUserRepository UserRepository => _unitOfWork.UserRepository;
    
    public async Task CheckUser(Update update)
    {
        if (await UserRepository.Any(q => q.TelegramUserId == update.Message!.From!.Id)) return;

        var user = new User
        {
            UserName = update.Message!.From!.Username!,
            ChatId = update.Message.Chat.Id,
            TelegramUserId = update.Message.From.Id
        };

        await UserRepository.Insert(user);
        await _unitOfWork.Save();
    }

    public async Task<ICollection<Library.Models.Task>> GetUserTasks(long telegramUserId) => UserRepository.GetUser(telegramUserId).Result.Tasks;
}