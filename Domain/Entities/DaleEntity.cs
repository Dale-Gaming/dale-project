namespace Domain.Entities;

public abstract class DaleEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserCreatedId { get; set; }
    public Guid? UserUpdatedId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public bool Active { get; set; } = true;
}
