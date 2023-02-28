using Microsoft.EntityFrameworkCore;
using RedBook.Core.Repositories;
using RedBook.Core.UnitOfWork;

namespace RedBook.Core.EntityFramework
{
    public class EFUnitOfWork: IUnitOfWork
    {
        private readonly DbContext _dbContext;
        public EFUnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IUnitOfWorkManager Begin()
        {
            return new EfUnitOfWorkManager(_dbContext);
        }

        public IRepositoryBase<TEntity> GetRepository<TEntity>() where TEntity : class => new RepositoryBase<TEntity>(_dbContext);
    }
}
