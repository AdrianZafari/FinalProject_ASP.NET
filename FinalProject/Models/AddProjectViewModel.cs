using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models;

public class AddProjectViewModel
{
    [Required]
    [Display(Name = "Project Image")]
    public IFormFile? ProjectImage { get; set; }

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

    public string TimeLeft { get; set; } = "N/A";

    public List<string> ExistingClients { get; set; } = new List<string>();

    //public IEnumerable<SelectListItem> Clients { get; set; } = [];
    public IEnumerable<SelectListItem> Members { get; set; } = [];


}
