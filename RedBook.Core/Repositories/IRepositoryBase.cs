using RedBook.Core.EntityFramework;
using RedBook.Core.Models;
using System.Linq.Expressions;

namespace RedBook.Core.Repositories
{
    public interface IRepositoryBase<TEntity, TPrimaryKey> where TEntity : BaseEntity<TPrimaryKey> where TPrimaryKey : Type
    {
        IQueryable<TEntity> TrackableQuery();
        IQueryable<TEntity> UnTrackableQuery();

        // Create
        Task<TEntity> InsertAsync(TEntity entity);

        // Read
        Task<TEntity> GetByIdAsync(TPrimaryKey id);
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> where);
        Task<PagedModel<TEntity>> GetPagedAsync(PagedModel<TEntity>? pagedModel);

        // Update
        TEntity Update(TEntity entity);

        // Delete
        Task<TEntity> DeleteAsync(TPrimaryKey id);
        Task DeleteAsync(Expression<Func<TEntity, bool>> where);

        // Utilities
        Task SaveChangesAsync();

         void DetachAllEntities();
    }
}
