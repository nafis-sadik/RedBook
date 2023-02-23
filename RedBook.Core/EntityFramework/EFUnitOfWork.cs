using Microsoft.EntityFrameworkCore;
using RedBook.Core.Repositories;
using RedBook.Core.UnitOfWork;

namespace RedBook.Core.EntityFramework
{
    public class EFUnitOfWork<TDbContext> : IUnitOfWork where TDbContext : DbContext
    {
        private readonly TDbContext _dbContext;
        public EFUnitOfWork(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IUnitOfWorkManager Begin()
        {
            return new EfUnitOfWorkManager(_dbContext);
        }
    }
}
