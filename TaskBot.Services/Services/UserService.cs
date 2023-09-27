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
        if (await UserRepository.Any(q => q.UserName == update.Message!.Chat.Username)) return;

        var user = new User
        {
            UserName = update.Message!.Chat.Username!,
            ChatId = update.Message.Chat.Id
        };

        await UserRepository.Insert(user);
        await _unitOfWork.Save();
    }

    public async Task<ICollection<Library.Models.Task>> GetUserTasks(long userChatId) => UserRepository.GetUser(userChatId).Result.Tasks;
}