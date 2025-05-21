using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models;

public class EditMemberViewModel
{

    public string Id { get; set; } = null!;

    [Required]
    [Display(Name = "First Name")]
    [RegularExpression(@"^[A-Z](?:[a-zA-Z]+(?:-[a-zA-Z]+)*)?$", ErrorMessage = "First name must start with a capital letter and be alphabetic.")]
    public string FirstName { get; set; } = null!;

    [Required]
    [Display(Name = "Last Name")]
    [RegularExpression(@"^[A-Z](?:[a-zA-Z]+(?:-[a-zA-Z]+)*)?$", ErrorMessage = "Last name must start with a capital letter and be alphabetic.")]
    public string LastName { get; set; } = null!;

    [Required]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    public string Email { get; set; } = null!;

    [Phone]
    [Display(Name = "Phone Number")]
    [RegularExpression(@"^\+?\d{7,15}$", ErrorMessage = "Phone number must be between 7 and 15 digits.")]
    public string? PhoneNumber { get; set; }

    [Display(Name = "Job Title")]
    [RegularExpression(@"^[\w\s\.,'-]{2,100}$", ErrorMessage = "Enter a valid job title.")]
    public string? JobTitle { get; set; }

    [RegularExpression(@"^[\w\s\.,'-]{5,100}$", ErrorMessage = "Enter a valid address.")]
    public string? Address { get; set; }

    [Display(Name = "Date of Birth")]
    [DataType(DataType.Date)]
    public DateTime? DateOfBirth { get; set; }

    [Display(Name = "Member Image")]
    public IFormFile? MemberImage { get; set; }
}
