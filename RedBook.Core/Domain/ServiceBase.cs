using Microsoft.Extensions.Logging;
using RedBook.Core.AutoMapper;
using RedBook.Core.UnitOfWork;

namespace RedBook.Core.Domain
{
    public class ServiceBase
    {
        private IUnitOfWorkManager _unitOfWorkManager;
        public IUnitOfWorkManager UnitOfWorkManager
        {
            get
            {
                if (_unitOfWorkManager == null)
                    throw new ArgumentException("UnitOfWorkManager must be assigned before used.");

                return _unitOfWorkManager;
            }
            set { _unitOfWorkManager = value; }
        }
        public ILogger Logger { protected get; set; }
        public IObjectMapper Mapper { get; set; }
        public ServiceBase(ILogger logger, IObjectMapper mapper/*, IClaimsPrincipalAccessor accessor*/)
        {

        }
    }
}
