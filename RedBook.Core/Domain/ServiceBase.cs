using Microsoft.Extensions.Logging;
using RedBook.Core.AutoMapper;
using RedBook.Core.Security;
using RedBook.Core.UnitOfWork;
using System.Security.Claims;

namespace RedBook.Core.Domain
{
    public class ServiceBase
    {
        private IUnitOfWork _unitOfWorkManager;
        public IUnitOfWork UnitOfWorkManager
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

        private IClaimsPrincipalAccessor ClaimsPrincipalAccessor { get; }
        protected ClaimsPrincipal? User
        {
            get { return ClaimsPrincipalAccessor?.GetCurrentPrincipal(); }
        }

        public ServiceBase(ILogger logger, IObjectMapper mapper, IClaimsPrincipalAccessor accessor, IUnitOfWork unitOfWork)
        {
            Logger = logger;
            Mapper = mapper;
            ClaimsPrincipalAccessor = accessor;
            _unitOfWorkManager = unitOfWork;
        }
    }
}
