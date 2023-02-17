using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBook.Core.UnitOfWork
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// Starts a unit of work.
        /// </summary>
        IUnitOfWorkManager Begin();
    }
}
