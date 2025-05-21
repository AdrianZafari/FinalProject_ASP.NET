using Microsoft.AspNetCore.Mvc.Rendering;

namespace FinalProject.Models;

public class ProjectsViewModel
{
    public IEnumerable<ProjectViewModel> Projects { get; set; } = [];
    public AddProjectViewModel AddProjectFormData { get; set; } = new();
    public EditProjectViewModel EditProjectFormData { get; set; } = new();

    public IEnumerable<SelectListItem> Statuses { get; set; } = new List<SelectListItem>();
}
