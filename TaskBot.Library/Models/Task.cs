namespace TaskBot.Library.Models;

public class Task : EntityBase
{
    public string Number { get; set; } = null!;

    public string? Description { get; set; }

    public string TaskName => Number + "-" + Description;

    public string TaskClose => IsClosed ? "Закрыта" : "Открыта";

    public string? Result { get; set; }

    public bool IsClosed { get; set; }

    public DateTime CreateDt { get; set; }

    public DateTime CloseDt { get; set; }

    public User User { get; set; }
}