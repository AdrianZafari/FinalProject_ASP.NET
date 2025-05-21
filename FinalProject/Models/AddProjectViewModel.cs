using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models;

public class AddProjectViewModel
{

    [Display(Name = "Project Image")]
    public IFormFile? ProjectImage { get; set; }

    [Required(ErrorMessage = "Required")]
    [Display(Name = "Project Name")]
    [RegularExpression(@"^[A-Z][a-zA-Z]*(?:[ -][a-zA-Z]+)*$", ErrorMessage = "Invalid project name")]
    [StringLength(50, MinimumLength = 2)]
    public string ProjectName { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [Display(Name = "Client Name")]
    [StringLength(100, MinimumLength = 1)]
    [RegularExpression(@"^[A-Z][a-zA-Z0-9 .,'&@#()\-!]*$", ErrorMessage = "Invalid client name")]
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

    [Required(ErrorMessage = "Required")]
    public string? MemberId { get; set; }

    //public List<string> ExistingClients { get; set; } = new List<string>();

    [Required(ErrorMessage = "Required")]
    public IEnumerable<SelectListItem> Members { get; set; } = [];


}
