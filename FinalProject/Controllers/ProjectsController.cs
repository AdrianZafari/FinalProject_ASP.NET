using Business.DTOs;
using Business.Services;
using Domain.Extensions;
using Domain.Models;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace FinalProject.Controllers;

[Route("projects")]
public class ProjectsController(IProjectService projectService, IMemberService memberService, IStatusService statusService) : Controller
{
    private readonly IStatusService _statusService = statusService;
    private readonly IMemberService _memberService = memberService;
    private readonly IProjectService _projectService = projectService;


    public async Task<IActionResult> Index()
    {
        var viewModel = new ProjectsViewModel()
        {
            Statuses = await SetStatuses(),
            Projects = await SetProjectsAsync(),
            AddProjectFormData = new AddProjectViewModel
            {
                Members = await SetMembersAsync()
            },
            EditProjectFormData = new EditProjectViewModel
            {
                Members = await SetMembersAsync(),
                Statuses = await SetStatuses()
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
                              kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToArray()
                );

            return BadRequest(new { errors });
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
        formData.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
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


    [HttpPost("edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, EditProjectViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(kvp => kvp.Key,
                              kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToArray()
                );

            return BadRequest(new { errors });
        }

        if (id != model.Id)
        {
            ModelState.AddModelError(string.Empty, "Mismatched ID.");
            return RedirectToAction("Index");
        }

        if (!ModelState.IsValid)
        {
            TempData["ShowEditModal"] = true;
            TempData["EditErrors"] = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage));
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
            var existing = await _projectService.GetProjectAsync(id);
            if (existing.Succeeded && existing.Result != null)
            {
                imagePath = existing.Result.ProjectImage;
            }
        }

        var formData = model.MapTo<AddProjectFormData>();
        formData.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        formData.ProjectImage = imagePath;

        var result = await _projectService.UpdateProjectAsync(id, formData);
        if (!result.Succeeded)
        {
            TempData["EditErrors"] = new List<string> { result.Error! };
            TempData["ShowEditModal"] = true;
            return RedirectToAction("Index");
        }

        TempData["Success"] = "Project updated.";
        return RedirectToAction("Index");
    }


    // This was made by me with the help of ChatGPT
    private async Task<IEnumerable<ProjectViewModel>> SetProjectsAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var projectResult = await _projectService.GetAllProjectsAsync(userId!);

        if (!projectResult.Succeeded || projectResult.Result == null)
            return Enumerable.Empty<ProjectViewModel>();

        var projects = projectResult.Result;

        // Collect unique MemberIds across projects
        var memberIds = projects
            .Select(p => p.MemberId)
            .Where(id => !string.IsNullOrEmpty(id))
            .Distinct()
            .ToList();

        // Prepare member dictionary
        var memberDict = new Dictionary<string, Member>();

        foreach (var memberId in memberIds)
        {
            var memberResult = await _memberService.GetByIdAsync(memberId!);
            if (memberResult.Succeeded && memberResult.Result != null)
            {
                memberDict[memberId!] = memberResult.Result;
            }
        }

        return projects.Select(project =>
        {
            var member = project.MemberId != null && memberDict.ContainsKey(project.MemberId)
                ? memberDict[project.MemberId]
                : null;

            return new ProjectViewModel
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
                MemberId = project.MemberId,
                StatusId = project.StatusId?.ToString(),
                MemberImage = member?.MemberImage ?? "/images/users/user-template-male-green.svg"
            };
        });
    }

    [HttpPost("delete/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _projectService.DeleteProjectAsync(id);

        if (!result.Succeeded)
        {
            TempData["Error"] = result.Error;
        }
        else
        {
            TempData["Success"] = "Project deleted.";
        }

        return RedirectToAction("Index");
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

    private async Task<IEnumerable<SelectListItem>> SetStatuses()
    {
        var result = await _statusService.GetStatusesAsync();

        if (!result.Succeeded || result.Result == null)
            return Enumerable.Empty<SelectListItem>();


        return result.Result.Select(s => new SelectListItem
        {
            Value = s.Id.ToString(),
            Text = s.StatusName
        });
    }

}
