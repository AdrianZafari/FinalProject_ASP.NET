
namespace Business.DTOs;

public class AddMemberFormData
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string JobTitle { get; set; } = null!;
    public string? Address { get; set; } 
    public DateTime? DateOfBirth { get; set; }
    public string? MemberImage { get; set; }

    public string? UserId { get; set; }
}
