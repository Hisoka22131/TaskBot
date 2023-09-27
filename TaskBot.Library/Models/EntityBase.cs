namespace TaskBot.Library.Models;

public abstract class EntityBase : IEntity
{
    public Guid Id { get; set; } = new();
}