using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RedBook.Core.AutoMapper;
using RedBook.Core.Constants;
using RedBook.Core.Repositories;
using RedBook.Core.Security;
using RedBook.Core.UnitOfWork;
using System.Data;
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

        public interface IRequestingUser {
            public string UserId
            {
                get {
                    return "";
                }
                private set { }
            }
            int[] RoleIds
            {
                get
                {
                    return new int[] { };
                }
                private set { }
            }
        };
        private class RequestingUser: IRequestingUser
        {
            private readonly ClaimsPrincipal _claimsPrincipalAccessor;
            public RequestingUser(IClaimsPrincipalAccessor ClaimsPrincipalAccessor)
            {
                _claimsPrincipalAccessor = ClaimsPrincipalAccessor.GetCurrentPrincipal();
            }

            string IRequestingUser.UserId
            {
                get
                {
                    string? guid = _claimsPrincipalAccessor.Claims.FirstOrDefault(x => x.Type.Equals("UserId", StringComparison.InvariantCultureIgnoreCase))?.Value;
                    if (!string.IsNullOrWhiteSpace(guid))
                        return guid;
                    else
                        throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidToken);
                }
            }

            int[] IRequestingUser.RoleIds
            {
                get
                {
                    string? roleIds = _claimsPrincipalAccessor.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role, StringComparison.InvariantCultureIgnoreCase))?.Value;
                    if (!string.IsNullOrEmpty(roleIds))
                    {
                        string[] strArray = roleIds.Split(',');
                        return Array.ConvertAll(strArray, int.Parse);
                    }
                    else
                        throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidToken);
                }
            }
        }
        private RequestingUser _userInfo;
        public IRequestingUser User { get { return _userInfo; } private set { } }

        public ServiceBase(ILogger<ServiceBase> logger, IObjectMapper mapper, IClaimsPrincipalAccessor accessor, IUnitOfWorkManager unitOfWork)
        {
            Logger = logger;
            Mapper = mapper;
            User = new RequestingUser(accessor);
            UnitOfWorkManager = unitOfWork;
        }
        public ServiceBase(ILogger<ServiceBase> logger, IObjectMapper mapper, IClaimsPrincipalAccessor accessor, IUnitOfWorkManager unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            Logger = logger;
            Mapper = mapper;
            User = new RequestingUser(accessor);
            UnitOfWorkManager = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
