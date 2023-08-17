using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RedBook.Core.Repositories;
using RedBook.Core.UnitOfWork;

namespace RedBook.Core.EntityFramework
{
    public class EfUnitOfWork : IUnitOfWork
    {
        public readonly DbContext _dbContext;

        public EfUnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

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
        public void Dispose() => _dbContext.Dispose();
        public IRepositoryBase<TEntity> GetRepository<TEntity>() where TEntity : class => new RepositoryBase<TEntity>(_dbContext);
        public void SaveChanges() => _dbContext.SaveChanges();
        public async Task SaveChangesAsync() => await _dbContext.SaveChangesAsync();
    }
}
