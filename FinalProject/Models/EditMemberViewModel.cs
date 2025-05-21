using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models;

public class EditMemberViewModel
{

    public string Id { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [Display(Name = "First Name")]
    [RegularExpression(@"^[A-Z](?:[a-zA-Z]+(?:-[a-zA-Z]+)*)?$", ErrorMessage = "Invalid first name")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [Display(Name = "Last Name")]
    [RegularExpression(@"^[A-Z](?:[a-zA-Z]+(?:-[a-zA-Z]+)*)?$", ErrorMessage = "Invalid last name")]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [Phone]
    [Display(Name = "Phone Number")]
    [RegularExpression(@"^\+?\d{7,15}$", ErrorMessage = "Invalid phone number")]
    public string? PhoneNumber { get; set; }

    [Required(ErrorMessage = "Required")]
    [Display(Name = "Job Title")]
    [RegularExpression(@"^[\w\s\.,'-]{2,100}$", ErrorMessage = "Invalid job title")]
    public string? JobTitle { get; set; }

    [RegularExpression(@"^[\w\s\.,'-]{5,100}$", ErrorMessage = "Invalid address")]
    public string? Address { get; set; }

    [Display(Name = "Date of Birth")]
    [DataType(DataType.Date)]
    public DateTime? DateOfBirth { get; set; }

    [Display(Name = "Member Image")]
    public IFormFile? MemberImage { get; set; }
}
