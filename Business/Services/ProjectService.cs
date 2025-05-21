using Business.DTOs;
using Business.Models;
using Data.Entities;
using Data.Repositories;
using Domain.Extensions;
using Domain.Models;

namespace Business.Services;

public interface IProjectService
{
    Task<ProjectResult> CreateProjectAsync(AddProjectFormData formData);
    Task<ProjectResult> DeleteProjectAsync(string id);
    Task<ProjectResult<Project>> GetProjectAsync(string id);
    Task<ProjectResult<IEnumerable<Project>>> GetAllProjectsAsync(string userId);
    Task<ProjectResult> UpdateProjectAsync(string id, AddProjectFormData formData);
}

public class ProjectService(IProjectRepository projectRepository, IStatusService statusService) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IStatusService _statusService = statusService;

    public async Task<ProjectResult> CreateProjectAsync(AddProjectFormData formData)
    {

        if (formData == null)
        {
            return new ProjectResult { Succeeded = false, StatusCode = 400, Error = "Not all required fields are filled." };
        }

        var projectEntity = formData.MapTo<ProjectEntity>();
        var statusResult = await _statusService.GetStatusByIdAsync(1); // Status ID 1 is "STARTED" and the default for any project created
        var status = statusResult.Result;

        projectEntity.StatusId = status!.Id;

        var result = await _projectRepository.AddAsync(projectEntity);

        return result.Succeeded
            ? new ProjectResult { Succeeded = true, StatusCode = 201 }
            : new ProjectResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };

    }

    public async Task<ProjectResult<IEnumerable<Project>>> GetAllProjectsAsync(string userId)
    {
        var response = await _projectRepository.GetAllAsync //Fetches all projects for a specific user in order of creation by the userId.
            (
                orderByDescending: true,
                sortBy: x => x.Created,
                where: m => m.UserId == userId
            );

        if (!response.Succeeded || response.Result == null)
        {
            return new ProjectResult<IEnumerable<Project>>
            {
                Succeeded = false,
                StatusCode = 404,
                Error = "No projects found."
            };
        }

        var projects = response.Result.Select(p => new Project 
        {
            Id = p.Id,
            ProjectName = p.ProjectName,
            ProjectImage = p.ProjectImage,
            Description = p.Description,
            StartDate = p.StartDate,
            EndDate = p.EndDate,
            Budget = p.Budget,
            ClientName = p.ClientName,
            MemberId = p.MemberId, // Fetches the member assigned to the project, in the interest of time this is not a table of members
            StatusId = p.StatusId // The default status is "STARTED" and assigned on createion, this should always be 1 for a new project
        });

        return new ProjectResult<IEnumerable<Project>> { Succeeded = true, StatusCode = 200, Result = response.Result };
    }

    public async Task<ProjectResult<Project>> GetProjectAsync(string id)
    {
        var response = await _projectRepository.GetAsync
            (
                where: x => x.Id == id
            );

        return response.Succeeded
            ? new ProjectResult<Project> { Succeeded = true, StatusCode = 200, Result = response.Result }
            : new ProjectResult<Project> { Succeeded = false, StatusCode = 404, Error = $"Project '{id}' was not found." };
    }

    public async Task<ProjectResult> DeleteProjectAsync(string id)
    {
        var entityResult = await _projectRepository.GetEntityAsync(m => m.Id == id);

        if (!entityResult.Succeeded || entityResult.Result == null)
        {
            return new ProjectResult
            {
                Succeeded = false,
                StatusCode = 404,
                Error = "Member not found."
            };
        }

        var deleteResult = await _projectRepository.DeleteAsync(entityResult.Result);

        return deleteResult.Succeeded
            ? new ProjectResult { Succeeded = true, StatusCode = 200 }
            : new ProjectResult { Succeeded = false, StatusCode = 500, Error = "Failed to delete project." };
    }

    public async Task<ProjectResult> UpdateProjectAsync(string id, AddProjectFormData formData)
    {
        if (formData == null)
        {
            return new ProjectResult
            {
                Succeeded = false,
                StatusCode = 400,
                Error = "Form data is required."
            };
        }

        var getResult = await _projectRepository.GetEntityAsync(x => x.Id == id);

        if (!getResult.Succeeded || getResult.Result == null)
        {
            return new ProjectResult
            {
                Succeeded = false,
                StatusCode = 404,
                Error = $"Project with ID '{id}' not found."
            };
        }

        var existingEntity = getResult.Result;

        existingEntity.ProjectName = formData.ProjectName;
        existingEntity.ProjectImage = formData.ProjectImage;
        existingEntity.Description = formData.Description;
        existingEntity.StartDate = formData.StartDate;
        existingEntity.EndDate = formData.EndDate;
        existingEntity.Budget = formData.Budget;
        existingEntity.ClientName = formData.ClientName;
        existingEntity.MemberId = formData.MemberId; 
        existingEntity.StatusId = formData.StatusId; // Here the other statuses are available to be assigned

        var updateResult = await _projectRepository.UpdateAsync(existingEntity);

        return updateResult.Succeeded
            ? new ProjectResult { Succeeded = true, StatusCode = 200 }
            : new ProjectResult { Succeeded = false, StatusCode = updateResult.StatusCode, Error = updateResult.Error };
    }


}
