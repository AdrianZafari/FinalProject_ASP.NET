﻿using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email", Prompt = "Enter email address")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [DataType(DataType.Password)]
    [Display(Name = "Password", Prompt = "Enter password")]
    public string Password { get; set; } = null!;


    public bool RememberMe { get; set; }
}
