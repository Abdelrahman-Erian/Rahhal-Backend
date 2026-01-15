

using Rahhal.Models;

namespace Rahhal.Data.Repository
{
    public interface IRepository<TEntity> where TEntity : BaseModel
    {
        Task AddAsync(TEntity entity);
        Task SoftDeleteAsync(TEntity entity);
        Task HardDeleteAsync(TEntity entity);
        Task Update(TEntity entit , params string[] properties );
        Task<TEntity?> GetByIDAsync(Guid id);
        IQueryable<TEntity> GetAll();
        Task SaveChangesAsync();
    }
}
