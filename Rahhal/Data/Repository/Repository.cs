

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Rahhal.Common.Views;
using Rahhal.Models;
using System.Collections.Generic;

namespace Rahhal.Data.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseModel
    {
        Context _context;
        DbSet<TEntity> _dbSet;
        readonly UserState _userState;
        protected string[] defaultExcludedEditProperties = new string[]
       { nameof(BaseModel.ID), nameof(BaseModel.CreatedBy), nameof(BaseModel.CreatedDate), nameof(BaseModel.IsDeleted) };

        protected string[] defaultExcludedDeleteProperties = new string[]
            { nameof(BaseModel.ID), nameof(BaseModel.CreatedBy), nameof(BaseModel.CreatedDate) };

        public Repository(Context Context , UserInfoProvider userInfoProvider )
        {
            _context = Context;
            _dbSet = _context.Set<TEntity>();
            _userState = userInfoProvider.userState;
        }

        public async Task AddAsync(TEntity entity)
        {
            entity.CreatedDate = DateTime.Now;
            entity.UpdatedDate = DateTime.Now;
            await _dbSet.AddAsync(entity);
        }

        public async Task HardDeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task SoftDeleteAsync(TEntity entity)
        {
            entity.IsDeleted = true;
            _dbSet.Update(entity);
        }
        public async Task Update(TEntity entity, params string[] properties)
        {
            var local = _dbSet.Local
          .FirstOrDefault(entry => entry.ID == entity.ID);

            EntityEntry entry;

            if (local is null)
                entry = _context.Entry(entity);
            else
                entry = _context.ChangeTracker.Entries<TEntity>().FirstOrDefault(e => e.Entity.ID == entity.ID);

            foreach (var prop in entry.Properties)
            {
                if (properties.Contains(prop.Metadata.Name))
                {
                    prop.CurrentValue = entity.GetType().GetProperty(prop.Metadata.Name).GetValue(entity);
                    prop.IsModified = true;
                }
            }

            entity.UpdatedBy = _userState.UserID;
            entity.UpdatedDate = DateTime.UtcNow;

            await Task.CompletedTask;
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
