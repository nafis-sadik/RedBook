using RedBook.Core.Repositories;

namespace RedBook.Core.UnitOfWork
{
    public interface IUnitOfWorkManager: IDisposable
    {
        /// <summary>
        /// Creates repository object for given entity using the DbContext of UnitOfWork
        /// </summary>
        IRepositoryBase<TEntity> GetRepository<TEntity>() where TEntity : class;

        /// <summary>
        /// Saves all changes until now in this unit of work.
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// Saves all changes until now in this unit of work.
        /// </summary>
        Task SaveChangesAsync();
    }
}
