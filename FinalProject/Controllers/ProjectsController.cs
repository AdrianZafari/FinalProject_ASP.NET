using Business.DTOs;
using Business.Services;
using Domain.Extensions;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace FinalProject.Controllers;

public class ProjectsController(IProjectService projectService, IMemberService memberService) : Controller
{
    private readonly IMemberService _memberService = memberService;
    private readonly IProjectService _projectService = projectService;

    [Route("/projects")]
    public async Task<IActionResult> Index()
    {
        var viewModel = new ProjectsViewModel()
        {
            Projects = SetProjects(),
            AddProjectFormData = new AddProjectViewModel
            {
                Members = await SetMembersAsync()
            },
            EditProjectFormData = new EditProjectViewModel
            {
                Members = await SetMembersAsync(),
                Statuses = SetStatuses()
            }
        };

        return View(viewModel);
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AddProjectViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(kvp => kvp.Key,
                              kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage))
                .ToArray();

            TempData["CreateErrors"] = errors;
            TempData["ShowCreateModal"] = true;

            return RedirectToAction("Index");
        }

        string? imagePath = null;
        if (model.ProjectImage != null && model.ProjectImage.Length > 0)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ProjectImage.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.ProjectImage.CopyToAsync(stream);
            }

            imagePath = $"/uploads/{uniqueFileName}";
        }
        else
        {
            imagePath = "/images/projects/project-template-purple.svg";
        }

        var formData = model.MapTo<AddProjectFormData>();
        formData.ProjectImage = imagePath;

        var result = await _projectService.CreateProjectAsync(formData);

        if (!result.Succeeded)
        {
            TempData["CreateErrors"] = new List<string> { result.Error! };
            TempData["ShowCreateModal"] = true;
            return RedirectToAction("Index");
        }

        return RedirectToAction("Index");
    }




    [HttpPost]
    public IActionResult Update(EditProjectViewModel model)
    {
        return View();
    }
    [HttpDelete]
    public IActionResult Delete(string id)
    {
        return View();
    }


    //private IEnumerable<ProjectViewModel> SetProjects(){
    //    var projects = new List<ProjectViewModel>();

    //    projects.Add(new ProjectViewModel
    //    {
    //        ProjectId = Guid.NewGuid().ToString(),
    //        ProjectName = "Website Redesign",
    //        ClientName = "GitLabs Inc.",
    //        ProjectImage = "/images/projects/project-template-purple.svg",
    //        Description = "<p>It is <strong>necessary</strong> to develop a website redesign in a corporate style.</p>",
    //        TimeLeft = "1 week left",
    //        Members = ["/images/users/user-template-male-green.svg"]
    //    }); 

    //    return projects;
    //}

    private IEnumerable<ProjectViewModel> SetProjects()
    {
        var projectResult = _projectService.GetProjectsAsync().Result;
        if (!projectResult.Succeeded || projectResult.Result == null)
            return Enumerable.Empty<ProjectViewModel>();

        return projectResult.Result.Select(project => new ProjectViewModel
        {
            ProjectId = project.Id,
            ProjectName = project.ProjectName,
            ClientName = project.ClientName ?? "Unknown Client",
            Description = project.Description!,
            ProjectImage = string.IsNullOrEmpty(project.ProjectImage)
                ? "/images/projects/project-template-purple.svg"
                : project.ProjectImage,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            Budget = project.Budget,
            TimeLeft = project.EndDate.HasValue
                ? $"{(project.EndDate.Value - DateTime.Now).Days} days left"
                : "N/A",
            Members = new List<MemberViewModel>
        {
            new MemberViewModel
            {
                Id = project.Member?.Id ?? "",
                FirstName = project.Member?.FirstName ?? "Unknown",
                LastName = project.Member?.LastName ?? "Member",
                Email = project.Member?.Email ?? "N/A",
                PhoneNumber = project.Member?.PhoneNumber ?? "N/A",
                JobTitle = project.Member?.JobTitle ?? "N/A",
                Address = project.Member?.Address ?? "N/A",
                DateOfBirth = project.Member?.DateOfBirth ?? default,
                MemberImage = string.IsNullOrEmpty(project.Member?.MemberImage)
                    ? "/images/users/user-template-male-green.svg"
                    : project.Member.MemberImage
            }
        }
        });
    }




    private async Task<IEnumerable<SelectListItem>> SetMembersAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _memberService.GetAllAsync(userId!);

        if (!result.Succeeded || result.Result == null)
        {
            return Enumerable.Empty<SelectListItem>();
        }

        return result.Result.Select(m => new SelectListItem
        {
            Value = m.Id,
            Text = $"{m.FirstName} {m.LastName}"
        });
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
