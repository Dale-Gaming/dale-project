namespace Domain.Entities;

public class UserProfile : DaleEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public string ZipCode { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;

    public DateTime? BirthDate { get; set; }
    public string? Phone { get; set; }
}
