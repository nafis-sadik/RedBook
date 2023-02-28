using RedBook.Core.Repositories;

namespace RedBook.Core.UnitOfWork
{
    public interface IUnitOfWorkManager: IDisposable
    {
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
