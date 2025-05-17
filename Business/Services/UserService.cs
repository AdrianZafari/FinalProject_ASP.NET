using Business.DTOs;
using Business.Models;
using Data.Entities;
using Data.Repositories;
using Domain.Extensions;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;
using System.Diagnostics;

namespace Business.Services;

public interface IUserService
{
    Task<UserResult> AddUserToRole(string userId, string roleName);
    Task<UserResult> CreateUserAsync(SignUpFormData formData, string roleName = "User");
    Task<UserResult<AddUserFormData>> GetUserAsync(string userId);
    Task<UserResult<IEnumerable<AddUserFormData>>> GetUsersAsync();
}

public class UserService(IUserRepository userRepository, UserManager<UserEntity> userManager, RoleManager<IdentityRole> roleManager) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;

    public async Task<UserResult<IEnumerable<AddUserFormData>>> GetUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();

        var dtoList = users.Result!.Select(user => new AddUserFormData
        {
            Id = user.Id,
            FirstName = user.FirstName ?? "",
            LastName = user.LastName ?? "",
            Email = user.Email ?? "",
            PhoneNumber = user.PhoneNumber ?? "",
            JobTitle = user.JobTitle ?? "",
            Address = user.Address ?? "",
            DateOfBirth = user.DateOfBirth ?? default,
            UserImage = user.UserImage ?? ""
        });

        return users.Succeeded
            ? new UserResult<IEnumerable<AddUserFormData>> { Succeeded = true, StatusCode = 200, Result = dtoList }
            : new UserResult<IEnumerable<AddUserFormData>> { Succeeded = false, StatusCode = 404, Error = "No users were found." };
    }

    public async Task<UserResult<AddUserFormData>> GetUserAsync(string userId)
    {
        var user = await _userManager.Users
            .Where(u => u.Id == userId)
            .Select(u => new AddUserFormData
            {
                Id = u.Id,
                FirstName = u.FirstName!,
                LastName = u.LastName!,
                Email = u.Email!,
                PhoneNumber = u.PhoneNumber!,
                JobTitle = u.JobTitle!,
                Address = u.Address!,
                DateOfBirth = u.DateOfBirth!,
                UserImage = u.UserImage!
            })
            .FirstOrDefaultAsync();

        if (user == null)
        {
            return new UserResult<AddUserFormData>
            {
                Succeeded = false,
                StatusCode = 404,
                Error = "User not found"
            };
        }

        return new UserResult<AddUserFormData>
        {
            Succeeded = true,
            StatusCode = 200,
            Result = user
        };
    }

    public async Task<UserResult> AddUserToRole(string userId, string roleName)
    {
        if (!await _roleManager.RoleExistsAsync(roleName))
            return new UserResult { Succeeded = false, StatusCode = 404, Error = "Role does not exist." };

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return new UserResult { Succeeded = false, StatusCode = 404, Error = "User does not exist." };

        var result = await _userManager.AddToRoleAsync(user, roleName);
        return result.Succeeded
            ? new UserResult { Succeeded = true, StatusCode = 200 }
            : new UserResult { Succeeded = false, StatusCode = 500, Error = "Failed to add User to Role" };
    }

    public async Task<UserResult> CreateUserAsync(SignUpFormData formData, string roleName = "User")
    {
        if (formData == null)
            return new UserResult { Succeeded = false, StatusCode = 400, Error = "Not all required fields are filled." };

        if (!formData.Terms)
            return new UserResult { Succeeded = false, StatusCode = 400, Error = "Terms and Conditions must be accepted." };

        var existsResult = await _userRepository.ExistsAsync(x => x.Email == formData.Email);
        if (existsResult.Succeeded)
            return new UserResult { Succeeded = false, StatusCode = 409, Error = "A user with this email already exists." };

        try
        {
            var userEntity = formData.MapTo<UserEntity>();

            var result = await _userManager.CreateAsync(userEntity, formData.Password);
            if (result.Succeeded)
            {
                var addToRoleResult = await AddUserToRole(userEntity.Id, roleName);
                return result.Succeeded
                    ? new UserResult { Succeeded = true, StatusCode = 201 }
                    : new UserResult { Succeeded = false, StatusCode = 201, Error = "User created but not added to role" };
            }

            return new UserResult { Succeeded = false, StatusCode = 500, Error = "An error occurred while creating the user." };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new UserResult { Succeeded = false, StatusCode = 500, Error = "An error occurred while creating the user." };
        }
    }
}
