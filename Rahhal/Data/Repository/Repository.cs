using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Rahhal.Common.Views;
using Rahhal.Models;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Rahhal.Data.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseModel, new()
    {
        Context _context;
        DbSet<TEntity> _dbSet;
        readonly UserState _userState;

        protected string[] defaultExcludedEditProperties = new string[]
        {
            nameof(BaseModel.ID), nameof(BaseModel.CreatedBy), nameof(BaseModel.CreatedDate),
            nameof(BaseModel.IsDeleted)
        };

        protected string[] defaultExcludedDeleteProperties = new string[]
            { nameof(BaseModel.ID), nameof(BaseModel.CreatedBy), nameof(BaseModel.CreatedDate) };

        public Repository(Context Context, UserInfoProvider userInfoProvider)
        {
            _context = Context;
            _dbSet = _context.Set<TEntity>();
            _userState = userInfoProvider.userState;
        }

        public async Task AddAsync(TEntity entity)
        {
            entity.CreatedDate = DateTime.UtcNow;
            entity.UpdatedDate = DateTime.UtcNow;
            await _dbSet.AddAsync(entity);
        }

        public async Task HardDeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public IQueryable<TEntity> Get()
        {
            return _dbSet.Where(entity => !entity.IsDeleted);
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return Get().Where(predicate);
        }

        public IQueryable<TEntity> GetAsSplit(Expression<Func<TEntity, bool>> predicate)
        {
            return Get().Where(predicate).AsSplitQuery();
        }


        public async Task SoftDeleteAsync(Guid entityId)
        {
            TEntity entity = new() { ID = entityId };
            entity.IsDeleted = true;
            entity.UpdatedBy = _userState.UserID ?? "System";
            entity.UpdatedDate = DateTime.UtcNow;
            Update(entity, nameof(entity.IsDeleted), nameof(entity.UpdatedBy), nameof(entity.UpdatedDate));
            await SaveChangesAsync();
        }

        public async Task Update(TEntity entity, params string[] properties)
        {
            var local = _dbSet.Local
                .FirstOrDefault(entry => entry.ID == entity.ID);

            EntityEntry entry;

            if (local is null)
                entry = _context.Entry(entity);
            else
                entry = _context.ChangeTracker.Entries<TEntity>().FirstOrDefault(e => e.Entity.ID == entity.ID)!;

            foreach (var prop in entry.Properties)
            {
                if (properties.Contains(prop.Metadata.Name))
                {
                    prop.CurrentValue = entity.GetType().GetProperty(prop.Metadata.Name)?.GetValue(entity);
                    prop.IsModified = true;
                }
            }

            entity.UpdatedBy = _userState.UserID;
            entity.UpdatedDate = DateTime.UtcNow;

            await Task.CompletedTask;
        }

        public Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.CreatedBy = _userState.UserID ?? "System";
                entity.CreatedDate = DateTime.UtcNow;
            }

            return _dbSet.AddRangeAsync(entities);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Get().AnyAsync(predicate);
        }


        public async Task<TEntity?> GetByIDAsync(Guid id)
        {
            return await _dbSet
                .FirstOrDefaultAsync(e => e.ID == id);
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbSet.Where(e => !e.IsDeleted);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}