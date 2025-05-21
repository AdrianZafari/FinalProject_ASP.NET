using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models;

public class ProjectViewModel
{
    public string ProjectId { get; set; } = string.Empty;
    public string? MemberId { get; set; }
    public  string? StatusId { get; set; } 


    [Required]
    [Display(Name = "Project Image")]
    public string? ProjectImage { get; set; }

    [Required]
    [Display(Name = "Project Name")]
    [StringLength(50, MinimumLength = 2)]
    public string ProjectName { get; set; } = null!;

    [Required]
    [Display(Name = "Client Name")]
    [StringLength(100, MinimumLength = 1)]
    public string ClientName { get; set; } = null!;

    [Required]
    [Display(Name = "Description")]
    [StringLength(250)]
    public string Description { get; set; } = null!;

    [Required]
    [Display(Name = "Start Date")]
    public DateTime StartDate { get; set; } = DateTime.Now;

    [Display(Name = "End Date")]
    public DateTime? EndDate { get; set; } = DateTime.Now;

    [Display(Name = "Budget")]
    public decimal? Budget { get; set; } = 0;

    public string? MemberImage { get; set; }

    public string TimeLeft { get; set; } = "N/A";

} 
