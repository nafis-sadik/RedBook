using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RedBook.Core.AutoMapper;
using RedBook.Core.Security;
using RedBook.Core.UnitOfWork;
using System.Security.Claims;

namespace RedBook.Core.Domain
{
    public class ServiceBase
    {
        private IHttpContextAccessor? _httpContextAccessor;
        public HttpContext? HttpContextAccessor {
            get {
                if(_httpContextAccessor != null)
                    return _httpContextAccessor.HttpContext;
                return null;
            }
            private set { }
        }

        private IUnitOfWorkManager _unitOfWorkManager;
        public IUnitOfWorkManager UnitOfWorkManager
        {
            get
            {
                if (_unitOfWorkManager == null)
                    throw new ArgumentException("UnitOfWorkManager must be assigned before used.");

                return _unitOfWorkManager;
            }
            private set { _unitOfWorkManager = value; }
        }
        public ILogger _logger;
        public ILogger Logger { get { return _logger; } private set { _logger = value; } }

        public IObjectMapper _mapper;
        public IObjectMapper Mapper { get { return _mapper; } private set { _mapper = value; } }

        private IClaimsPrincipalAccessor ClaimsPrincipalAccessor { get; }
        protected ClaimsPrincipal? User
        {
            get { return ClaimsPrincipalAccessor?.GetCurrentPrincipal(); }
        }
        public ServiceBase(ILogger<ServiceBase> logger, IObjectMapper mapper, IClaimsPrincipalAccessor accessor, IUnitOfWorkManager unitOfWork)
        {
            Logger = logger;
            Mapper = mapper;
            ClaimsPrincipalAccessor = accessor;
            UnitOfWorkManager = unitOfWork;
        }
        public ServiceBase(ILogger<ServiceBase> logger, IObjectMapper mapper, IClaimsPrincipalAccessor accessor, IUnitOfWorkManager unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            Logger = logger;
            Mapper = mapper;
            ClaimsPrincipalAccessor = accessor;
            UnitOfWorkManager = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
