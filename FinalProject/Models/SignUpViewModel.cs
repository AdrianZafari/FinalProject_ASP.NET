using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models;

public class SignUpViewModel
{
    [Required(ErrorMessage = "Required")]
    [DataType(DataType.Text)]
    [RegularExpression(@"^[A-Za-zÀ-ÖØ-öø-ÿ' -]{2,50}$", ErrorMessage = "Invalid First Name")]
    [Display(Name = "First Name", Prompt = "Enter first Name")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [DataType(DataType.Text)]
    [RegularExpression(@"^[A-Za-zÀ-ÖØ-öø-ÿ' -]{2,50}$", ErrorMessage = "Invalid Last Name")]
    [Display(Name = "Last Name", Prompt = "Enter last Name")]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email address")]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email", Prompt = "Enter email address")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+=\[{\]};:<>|./?,-]).{8,}$",
        ErrorMessage = "Password is too weak")]
    [DataType(DataType.Password)]
    [Display(Name = "Password", Prompt = "Enter password")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [Compare(nameof(Password), ErrorMessage = "Your passwords do not match")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password", Prompt = "Confirm password")]
    public string ConfirmPassword { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [Range(typeof(bool), "true", "true", ErrorMessage = "You must accept the Terms and Conditions.")]
    public bool Terms {  get; set; }
}
