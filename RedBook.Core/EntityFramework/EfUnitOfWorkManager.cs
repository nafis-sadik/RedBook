using Microsoft.EntityFrameworkCore;
using RedBook.Core.Repositories;
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

        public IUnitOfWork Begin()
        {
            return new EfUnitOfWork(_dbContext);
        }
    }
}
