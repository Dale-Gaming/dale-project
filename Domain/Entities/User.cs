namespace Domain.Entities;

public class User : DaleEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Nickname { get; set; } = string.Empty;

    public UserProfile? Profile { get; set; }
}
