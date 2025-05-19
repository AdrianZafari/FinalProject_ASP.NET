using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models;

public class ProjectViewModel
{
    public string ProjectId { get; set; } = string.Empty;

    [Required]

    public string ProjectImage { get; set; } = null!;

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string ProjectName { get; set; } = null!;

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string ClientName { get; set; } = null!;

    [Required]
    [StringLength(1000)]
    public string Description { get; set; } = null!;

    [Required]
    public string TimeLeft { get; set; } = null!;

    // Relationship
    public ICollection<string> Members { get; set; } = new List<string>(); // To be changed to MemberViewModel
} 
