using DataLayer.Context;
using DataLayer.Entities.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.GenericRepostitory
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly MarketPlaceDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(MarketPlaceDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public IQueryable<TEntity> GetQuery()
        {
            return _dbSet.AsQueryable();
        }
        // Add
        public async Task AddEntityAsync(TEntity entity)
        {
            entity.CreatedDate = DateTime.Now;
            entity.UpdatedDate = DateTime.Now;
            await _dbSet.AddAsync(entity);
        }
        // Add Range
        public async Task AddRangeEntitiesAsync(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                await AddEntityAsync(entity);
            }
        }
        // GetById
        public async Task<TEntity> GetByIdAsync(long Id)
        {
            return _dbSet.SingleOrDefault(x => x.Id == Id);
        }
        // Update
        public void UpdateEntity(TEntity entity)
        {
            entity.UpdatedDate = DateTime.Now;
            _dbSet.Update(entity);
        }
        // DeleteById
        public async Task DeleteEntityAsync(long Id)
        {
            DeleteEntity(await GetByIdAsync(Id));           
        }
        // Delete 
        public void DeleteEntity(TEntity entity)
        {
            entity.IsDeleted = true;
            _dbSet.Update(entity);
        }
        // DeleteById
        public async Task DeletePermanetAsync(long Id)
        {
            DeletePermanet(await GetByIdAsync(Id));
        }

        public void DeletePermanentEntities(List<TEntity> entities)
        {
            _context.RemoveRange(entities);
        }
        // Delete
        public void DeletePermanet(TEntity entity)
        {
            _dbSet.Remove(entity);
        }
        // SaveChanges
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async ValueTask DisposeAsync()
        {
            if (_context != null) await _context.DisposeAsync();
        }
    }
}
