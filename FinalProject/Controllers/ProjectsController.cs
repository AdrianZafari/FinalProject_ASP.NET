using Business.DTOs;
using Business.Services;
using Domain.Extensions;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace FinalProject.Controllers;

public class ProjectsController(IProjectService projectService) : Controller
{
    private readonly IProjectService _projectService = projectService;

    // Dummy Data for testing

    //public IActionResult Index()
    //{
    //    var viewModel = new ProjectsViewModel()
    //    {
    //        Projects = SetProjects(),
    //        AddProjectFormData = new AddProjectViewModel
    //        {
    //            Members = SetMembers()
    //        },
    //        EditProjectFormData = new EditProjectViewModel
    //        {
    //            Members = SetMembers(),
    //            Statuses = SetStatuses()
    //        }
    //    };

    //    return View(viewModel);
    //}

    [HttpPost]
    [Route("/projects")]
    public IActionResult Index() // Fix with the Database stuff when front end is connected to backend
    {
        var model = new ProjectsViewModel
        {
            Projects = SetProjects(),
            AddProjectFormData = new AddProjectViewModel
            {
                Members = SetMembers()
            },
            EditProjectFormData = new EditProjectViewModel
            {
                Members = SetMembers(),
                Statuses = SetStatuses()
            }
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddProjectViewModel model)
    {
        var addProjectFormData = model.MapTo<AddProjectFormData>();

        var result = await _projectService.CreateProjectAsync(addProjectFormData);

        return View();
    }
    [HttpPost]
    public IActionResult Update(EditProjectViewModel model)
    {
        return View();
    }
    [HttpPost]
    public IActionResult Delete(string id)
    {
        return View();
    }












    private IEnumerable<ProjectViewModel> SetProjects(){
        var projects = new List<ProjectViewModel>();

        projects.Add(new ProjectViewModel
        {
            ProjectId = Guid.NewGuid().ToString(),
            ProjectName = "Website Redesign",
            ClientName = "GitLabs Inc.",
            ProjectImage = "/images/projects/project-template-purple.svg",
            Description = "<p>It is <strong>necessary</strong> to develop a website redesign in a corporate style.</p>",
            TimeLeft = "1 week left",
            Members = ["/images/users/user-template-male-green.svg"]
        }); 

        return projects;
    }

    private IEnumerable<SelectListItem> SetMembers()
    {
        var members = new List<SelectListItem> { 
            new() { Value = "1", Text = "Adrian Zafari" },
            new() { Value = "2", Text = "Saiid Zafari" },
            new() { Value = "3", Text = "Hans Mattin-Lassei" }
        };

        return members;
    }

    private IEnumerable<SelectListItem> SetStatuses()
    {
        var statuses = new List<SelectListItem> {
            new() { Value = "1", Text = "STARTED", Selected = true},
            new() { Value = "2", Text = "COMPLETED" }
        };

        return statuses;
    }
}
