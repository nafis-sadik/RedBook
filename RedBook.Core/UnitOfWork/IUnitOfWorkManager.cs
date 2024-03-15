using Microsoft.EntityFrameworkCore.Storage;
using RedBook.Core.Repositories;

namespace RedBook.Core.UnitOfWork
{
    public interface IUnitOfWorkManager
    {
        /// <summary>
        /// Returns Repository Factory.
        /// </summary>
        public IRepositoryFactory GetRepositoryFactory();

        /// <summary>
        /// Starts a unit of work.
        /// </summary>
        public IDbContextTransaction Begin();

        /// <summary>
        /// Commits operations to database asynchronously.
        /// </summary>
        public Task CommitAsync();

        /// <summary>
        /// Rollbacks operations to last commit asynchronously.
        /// </summary>
        public Task RollbackAsync();
    }
}
