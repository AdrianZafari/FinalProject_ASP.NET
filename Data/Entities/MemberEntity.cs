﻿using Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class MemberEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    public string FirstName { get; set; } = null!;

    [Required]
    public string LastName { get; set; } = null!;

    [Required]
    public string Email { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    [Required]
    public string JobTitle { get; set; } = null!;

    public string? Address { get; set; }

    [DataType(DataType.Date)]
    public DateTime? DateOfBirth { get; set; }

    public string? MemberImage { get; set; }

    // Foreign Keys

    [ForeignKey(nameof(User))]
    public string? UserId { get; set; } = null!;
    public virtual UserEntity? User { get; set; } = null!;

    public virtual ICollection<ProjectEntity> Projects { get; set; } = [];
}
