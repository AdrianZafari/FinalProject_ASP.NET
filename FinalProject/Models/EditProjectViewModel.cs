using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models;

public class EditProjectViewModel
{
    public string? Id { get; set; } 

    [Display(Name = "Project Image")]
    public IFormFile? ProjectImage { get; set; }

    [Required(ErrorMessage = "Required")]
    [Display(Name = "Project Name")]
    [RegularExpression(@"^[A-Z][a-zA-Z]*(?:[ -][a-zA-Z]+)*$", ErrorMessage = "Invalid project name")]
    [StringLength(50, MinimumLength = 2)]
    public string ProjectName { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [Display(Name = "Client Name")]
    [RegularExpression(@"^[A-Z][a-zA-Z0-9 .,'&@#()\-!]*$", ErrorMessage = "Invalid client name")]
    [StringLength(100, MinimumLength = 1)]
    public string ClientName { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [Display(Name = "Description")]
    [StringLength(250, ErrorMessage = "Max 250 characters")]
    public string Description { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [Display(Name = "Start Date")]
    public DateTime StartDate { get; set; } = DateTime.Now;

    [Display(Name = "End Date")]
    public DateTime? EndDate { get; set; }

    [Display(Name = "Budget")]
    public decimal? Budget { get; set; } = 0;

    public string TimeLeft { get; set; } = "N/A";
    public string? MemberId { get; set; }
    public int? StatusId { get; set; }

    //public IEnumerable<SelectListItem> Clients { get; set; } = [];
    public IEnumerable<SelectListItem> Members { get; set; } = [];
    public IEnumerable<SelectListItem> Statuses { get; set; } = [];
}
