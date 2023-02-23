using Microsoft.EntityFrameworkCore;
using RedBook.Core.Repositories;
using RedBook.Core.UnitOfWork;

namespace RedBook.Core.EntityFramework
{
    public class EfUnitOfWorkManager : IUnitOfWorkManager
    {
        public readonly DbContext _dbContext;

        public EfUnitOfWorkManager(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IRepositoryBase<TEntity> GetRepository<TEntity>() where TEntity : class => new RepositoryBase<TEntity>(_dbContext);

        public void Dispose() => _dbContext.Dispose();
        public void SaveChanges() => _dbContext.SaveChanges();
        public async Task SaveChangesAsync() => await _dbContext.SaveChangesAsync();
    }
}
