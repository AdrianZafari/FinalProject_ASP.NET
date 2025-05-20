using Data.Contexts;
using Data.Entities;
using Data.Models;
using Domain.Models;
using System.Linq.Expressions;

namespace Data.Repositories;

public interface IProjectRepository : IBaseRepository<ProjectEntity, Project>
{
    Task<RepositoryResult<ProjectEntity>> GetEntityAsync(Expression<Func<ProjectEntity, bool>> where, params Expression<Func<ProjectEntity, object>>[] includes);
}
public class ProjectRepository(AppDbContext context) : BaseRepository<ProjectEntity, Project>(context), IProjectRepository
{
}
