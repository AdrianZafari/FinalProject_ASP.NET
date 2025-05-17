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
    Task<ProjectResult<IEnumerable<Project>>> GetProjectsAsync();
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
        var statusResult = await _statusService.GetStatusByIdAsync(1);
        var status = statusResult.Result;

        projectEntity.StatusId = status!.Id;

        var result = await _projectRepository.AddAsync(projectEntity);

        return result.Succeeded
            ? new ProjectResult { Succeeded = true, StatusCode = 201 }
            : new ProjectResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };

    }

    public async Task<ProjectResult<IEnumerable<Project>>> GetProjectsAsync()
    {
        var response = await _projectRepository.GetAllAsync
            (
                orderByDescending: true,
                sortBy: x => x.Created,
                where: null,
                include => include.User,
                include => include.Status,
                include => include.Client
            );

        return new ProjectResult<IEnumerable<Project>> { Succeeded = true, StatusCode = 200, Result = response.Result };
    }

    public async Task<ProjectResult<Project>> GetProjectAsync(string id)
    {
        var response = await _projectRepository.GetAsync
            (
                where: x => x.Id == id,
                include => include.User,
                include => include.Status,
                include => include.Client
            );
        return response.Succeeded
            ? new ProjectResult<Project> { Succeeded = true, StatusCode = 200, Result = response.Result }
            : new ProjectResult<Project> { Succeeded = false, StatusCode = 404, Error = $"Project '{id}' was not found." };
    }

    public async Task<ProjectResult> DeleteProjectAsync(string id)
    {
        var getResult = await _projectRepository.GetAsync(x => x.Id == id);

        if (!getResult.Succeeded || getResult.Result == null)
        {
            return new ProjectResult
            {
                Succeeded = false,
                StatusCode = 404,
                Error = $"Project with ID '{id}' not found."
            };
        }

        var deleteResult = await _projectRepository.DeleteAsync(getResult.Result.MapTo<ProjectEntity>());

        return deleteResult.Succeeded
            ? new ProjectResult { Succeeded = true, StatusCode = 200 }
            : new ProjectResult { Succeeded = false, StatusCode = deleteResult.StatusCode, Error = deleteResult.Error };
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

        var getResult = await _projectRepository.GetAsync(x => x.Id == id);

        if (!getResult.Succeeded || getResult.Result == null)
        {
            return new ProjectResult
            {
                Succeeded = false,
                StatusCode = 404,
                Error = $"Project with ID '{id}' not found."
            };
        }

        var existingEntity = getResult.Result.MapTo<ProjectEntity>();

        existingEntity.ProjectName = formData.ProjectName;
        existingEntity.ProjectImage = formData.ProjectImage;
        existingEntity.Description = formData.Description;
        existingEntity.StartDate = formData.StartDate;
        existingEntity.EndDate = formData.EndDate;
        existingEntity.Budget = formData.Budget;
        existingEntity.ClientId = formData.ClientId;
        existingEntity.UserId = formData.UserId;
        existingEntity.StatusId = formData.StatusId;

        var updateResult = await _projectRepository.UpdateAsync(existingEntity);

        return updateResult.Succeeded
            ? new ProjectResult { Succeeded = true, StatusCode = 200 }
            : new ProjectResult { Succeeded = false, StatusCode = updateResult.StatusCode, Error = updateResult.Error };
    }


}
