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
        Task<TEntity?> Get(int id);
        Task<TEntity?> Get(string id);
        Task<TEntity?> Get(object id);
        Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> where);

        // Update
        TEntity Update(TEntity entity);

        // Delete
        Task DeleteAsync(int id);
        void Delete(TEntity entity);
        Task DeleteAsync(Expression<Func<TEntity, bool>> where);

        // Utilities
        Task SaveChangesAsync();

         void DetachAllEntities();
    }
}
