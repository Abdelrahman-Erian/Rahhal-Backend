

using Microsoft.EntityFrameworkCore;
using Rahhal.Common.Views;
using Rahhal.Models;
using System.Linq.Expressions;

namespace Rahhal.Data.Repository
{
    public interface IRepository<TEntity> where TEntity : BaseModel
    {
        Task AddAsync(TEntity entity);
        Task SoftDeleteAsync(Guid entityId);
        Task HardDeleteAsync(TEntity entity);
        Task Update(TEntity entit , params string[] properties );
        Task<TEntity?> GetByIDAsync(Guid id);
         IQueryable<TEntity> Get();
        IQueryable<TEntity> GetAll();
        Task SaveChangesAsync();
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> GetAsSplit(Expression<Func<TEntity, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

    }
}
