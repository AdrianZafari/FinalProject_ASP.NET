using Data.Contexts;
using Data.Entities;
using Data.Models;
using Domain.Models;
using System.Linq.Expressions;

namespace Data.Repositories;

public interface IClientRepository : IBaseRepository<ClientEntity, Client>
{
    Task<RepositoryResult<ClientEntity>> GetEntityAsync(Expression<Func<ClientEntity, bool>> where, params Expression<Func<ClientEntity, object>>[] includes);
}

public class ClientRepository(AppDbContext context) : BaseRepository<ClientEntity, Client>(context), IClientRepository
{
}
