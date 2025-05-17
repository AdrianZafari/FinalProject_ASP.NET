using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models;

public class SignUpViewModel
{
    [Required]
    [DataType(DataType.Text)]
    [RegularExpression(@"^[A-Za-zÀ-ÖØ-öø-ÿ' -]{2,50}$", ErrorMessage = "First name can only contain letters, spaces, apostrophes, and hyphens.")]
    [Display(Name = "First Name", Prompt = "Enter first Name")]
    public string FirstName { get; set; } = null!;

    [Required]
    [DataType(DataType.Text)]
    [RegularExpression(@"^[A-Za-zÀ-ÖØ-öø-ÿ' -]{2,50}$", ErrorMessage = "Last name can only contain letters, spaces, apostrophes, and hyphens.")]
    [Display(Name = "Last Name", Prompt = "Enter last Name")]
    public string LastName { get; set; } = null!;

    [Required]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email address")]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email", Prompt = "Enter email address")]
    public string Email { get; set; } = null!;

    [Required]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+=\[{\]};:<>|./?,-]).{8,}$",
        ErrorMessage = "Password must be at least 8 characters and include uppercase, lowercase, number, and special character.")]
    [DataType(DataType.Password)]
    [Display(Name = "Password", Prompt = "Enter password")]
    public string Password { get; set; } = null!;

    [Required]
    [Compare(nameof(Password))]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password", Prompt = "Confirm password")]
    public string ConfirmPassword { get; set; } = null!;

    [Required]
    [Range(typeof(bool), "true", "true", ErrorMessage = "You must accept the Terms and Conditions.")]
    public bool Terms {  get; set; }
}
