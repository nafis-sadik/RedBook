using RedBook.Core.Repositories;

namespace RedBook.Core.UnitOfWork
{
    public interface IUnitOfWorkManager
    {
        /// <summary>
        /// Starts a unit of work.
        /// </summary>
        IUnitOfWork Begin();
    }
}
