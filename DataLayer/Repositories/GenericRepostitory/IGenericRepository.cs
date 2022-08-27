using DataLayer.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.GenericRepostitory
{
    public interface IGenericRepository<TEntity> : IAsyncDisposable where TEntity : BaseEntity
    {
        IQueryable<TEntity> GetQuery();
        //IQueryable<TEntity> GetAll(int pageIndex, int pageSize);
        //IQueryable<TEntity> GetAll(IEnumerable<TEntity> entities);
        Task AddEntityAsync(TEntity entity); // ADD
        Task<TEntity> GetByIdAsync(long Id); // GetById
        void UpdateEntity(TEntity entity); // Update
        Task DeleteEntityAsync(long id);     // Delete
        Task DeletePermanetAsync(long id);   // Delete
        void DeleteEntity(TEntity entity); // Delete
        void DeletePermanet(TEntity entity); // Delete
        Task SaveChangesAsync(); // Savechanges
    }
}
