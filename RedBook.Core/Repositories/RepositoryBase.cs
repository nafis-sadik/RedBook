using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RedBook.Core.Models;
using System.Linq.Expressions;

namespace RedBook.Core.Repositories
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public RepositoryBase(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> TrackableQuery() => _dbSet.AsQueryable();
        public virtual IQueryable<TEntity> UnTrackableQuery() => _dbSet.AsNoTracking().AsQueryable();

        // Create
        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            EntityEntry<TEntity> entityEntry = await _dbSet.AddAsync(entity);
            if (entityEntry.State == EntityState.Added) return entityEntry.Entity;
            else throw new InvalidOperationException($"Failed to insert {entity}");
        }

        // Read
        public async Task<TEntity?> GetByIdAsync(object id) => await _dbSet.FindAsync(id);
        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> query) => await UnTrackableQuery().Where(query).ToListAsync();
        
        // Update
        public virtual TEntity Update(TEntity entity) => _dbSet.Update(entity).Entity;

        // Delete
        public async Task DeleteAsync(object id)
        {
            var targetRow = await GetByIdAsync(id);
            if (targetRow != null)
                _dbSet.Remove(targetRow);
            else
                throw new ArgumentException($"Object with identifier {id} was not found");
        }

        public async Task DeleteAsync(Expression<Func<TEntity, bool>> where)
        {
            var objects = await UnTrackableQuery().Where(where).ToArrayAsync();
            foreach (var obj in objects)
            {
                _dbSet.Remove(obj);
            }

            await SaveChangesAsync();
        }

        // Utilities
        public async Task SaveChangesAsync() => await _dbContext.SaveChangesAsync();

        public async Task Dispose() => await _dbContext.DisposeAsync();

        public void DetachAllEntities()
        {
            IEnumerable<EntityEntry> changedEntriesCopy = _dbContext.ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Modified
                        || x.State == EntityState.Added
                        || x.State == EntityState.Deleted);
            foreach (var entity in changedEntriesCopy)
            {
                entity.State = EntityState.Detached;
            }
        }
    }
}
