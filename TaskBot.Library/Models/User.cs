namespace TaskBot.Library.Models;

public class User : EntityBase
{
    public string UserName { get; set; } = null!;
    
    public long ChatId { get; set; }

    public ICollection<Task> Tasks { get; set; }
}