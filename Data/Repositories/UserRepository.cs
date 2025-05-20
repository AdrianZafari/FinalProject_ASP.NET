using Data.Contexts;
using Data.Entities;
using Data.Models;
using Domain.Models;
using System.Linq.Expressions;

namespace Data.Repositories;

public interface IUserRepository : IBaseRepository<UserEntity, User>
{
    Task<RepositoryResult<UserEntity>> GetEntityAsync(Expression<Func<UserEntity, bool>> where, params Expression<Func<UserEntity, object>>[] includes);
}
public class UserRepository(AppDbContext context) : BaseRepository<UserEntity, User>(context), IUserRepository
{
}