using RedBook.Core.EntityFramework;
using RedBook.Core.Models;
using System.Linq.Expressions;

namespace RedBook.Core.Repositories
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        IQueryable<TEntity> TrackableQuery();
        IQueryable<TEntity> UnTrackableQuery();

        // Create
        Task<TEntity> InsertAsync(TEntity entity);

        // Read
        Task<TEntity?> GetByIdAsync(object id);
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> where);

        // Update
        TEntity Update(TEntity entity);

        // Delete
        Task DeleteAsync(object id);
        Task DeleteAsync(Expression<Func<TEntity, bool>> where);

        // Utilities
        Task SaveChangesAsync();

         void DetachAllEntities();
    }
}
