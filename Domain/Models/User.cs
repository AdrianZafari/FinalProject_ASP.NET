namespace Domain.Models;

public class User
{
    public string Id { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? JobTitle { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? UserImage { get; set; } = null!;
    public string? Address { get; set; } = null!;
    public DateTime? DateOfBirth { get; set; }

}
