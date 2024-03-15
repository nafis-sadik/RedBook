using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RedBook.Core.UnitOfWork;

namespace RedBook.Core.EntityFramework
{
    public class EFUnitOfWorkManager: IUnitOfWorkManager
    {
        private readonly DbContext _dbContext;
        public EFUnitOfWorkManager(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IRepositoryFactory GetRepositoryFactory() => new EFRepositoryFactory(_dbContext);
        public IDbContextTransaction Begin() => _dbContext.Database.BeginTransaction();
        public async Task CommitAsync() => await _dbContext.Database.CommitTransactionAsync();
        public async Task RollbackAsync() => await _dbContext.Database.RollbackTransactionAsync();
    }
}
