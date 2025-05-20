using Business.DTOs;
using Business.Models;
using Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Business.Services;

public interface IAuthService
{
    Task<AuthResult> SignInAsync(SignInFormData formData);
    Task<AuthResult> SignOutAsync();
    Task<AuthResult> SignUpAsync(SignUpFormData formData);
}

public class AuthService(IUserService userService, SignInManager<UserEntity> signInManager, UserManager<UserEntity> userManager) : IAuthService
{
    private readonly IUserService _userService = userService;
    private readonly SignInManager<UserEntity> _signInManager = signInManager;
    private readonly UserManager<UserEntity> _userManager = userManager;

    public async Task<AuthResult> SignInAsync(SignInFormData formData)
    {
        if (formData == null)
            return new AuthResult { Succeeded = false, StatusCode = 400, Error = "Not all required fields are filled." };

        // Find user by email
        var user = await _userManager.FindByEmailAsync(formData.Email);
        if (user == null)
            return new AuthResult { Succeeded = false, StatusCode = 401, Error = "Invalid email or password." };

        var result = await _signInManager.PasswordSignInAsync(
            user,              // <-- user instance, not string username. I debugged this with GPT, this solution worked, I'm not touching it.
            formData.Password,
            formData.RememberMe,
            lockoutOnFailure: false
        );

        return result.Succeeded
            ? new AuthResult { Succeeded = true, StatusCode = 200 }
            : new AuthResult { Succeeded = false, StatusCode = 401, Error = "Invalid email or password." };
    }

    public async Task<AuthResult> SignUpAsync(SignUpFormData formData)
    {
        if (formData == null)
            return new AuthResult { Succeeded = false, StatusCode = 400, Error = "Not all required fields are filled." };

        var result = await _userService.CreateUserAsync(formData);
        return result.Succeeded
            ? new AuthResult { Succeeded = true, StatusCode = 201 }
            : new AuthResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
    }

    public async Task<AuthResult> SignOutAsync()
    {
        await _signInManager.SignOutAsync();
        return new AuthResult { Succeeded = true, StatusCode = 200 };
    }
}