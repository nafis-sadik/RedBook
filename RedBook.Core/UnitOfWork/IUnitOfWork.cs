using RedBook.Core.Repositories;

namespace RedBook.Core.UnitOfWork
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// Starts a unit of work.
        /// </summary>
        IUnitOfWorkManager Begin();

        /// <summary>
        /// Creates repository object for given entity using the DbContext of UnitOfWork
        /// </summary>
        IRepositoryBase<TEntity> GetRepository<TEntity>() where TEntity : class;
    }
}
