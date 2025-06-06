﻿
namespace Domain.Models;

public class Member
{
    public string Id { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; } = null!;
    public string JobTitle { get; set; } = null!;
    public string? Address { get; set; } = null!;
    public DateTime? DateOfBirth { get; set; }
    public string? MemberImage { get; set; } = null!;
}
