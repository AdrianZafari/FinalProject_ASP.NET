using Data.Contexts;
using Data.Entities;
using Data.Models;
using Domain.Models;
using System.Linq.Expressions;

namespace Data.Repositories;

public interface IMemberRepository : IBaseRepository<MemberEntity, Member>
{
    Task<RepositoryResult<MemberEntity>> GetEntityAsync(Expression<Func<MemberEntity, bool>> where, params Expression<Func<MemberEntity, object>>[] includes);
}

public class MemberRepository(AppDbContext context) : BaseRepository<MemberEntity, Member>(context), IMemberRepository
{
}


