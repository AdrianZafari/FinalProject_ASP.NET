﻿using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models;

public class MemberViewModel
{
    public string Id { get; set; } = string.Empty;


    public string? MemberImage { get; set; }

    [Required]
    [RegularExpression(@"^[A-Z][a-zA-Z]{1,49}$", ErrorMessage = "First name must start with a capital letter and be alphabetic.")]
    public string FirstName { get; set; } = null!;

    [Required]
    [RegularExpression(@"^[A-Z][a-zA-Z]{1,49}$", ErrorMessage = "Last name must start with a capital letter and be alphabetic.")]
    public string LastName { get; set; } = null!;

    [Required]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    public string Email { get; set; } = null!;


    [Phone(ErrorMessage = "Enter a valid phone number.")]
    [RegularExpression(@"^\+?\d{7,15}$", ErrorMessage = "Phone number must be between 7 and 15 digits.")]
    public string? PhoneNumber { get; set; }

    [Required]
    [RegularExpression(@"^[\w\s\.,'-]{2,100}$", ErrorMessage = "Enter a valid job title.")]
    public string JobTitle { get; set; } = null!;


    [RegularExpression(@"^[\w\s\.,'-]{5,100}$", ErrorMessage = "Enter a valid address.")]
    public string? Address { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Date of Birth")]
    public DateTime? DateOfBirth { get; set; }
}
