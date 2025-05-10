﻿using Microsoft.AspNetCore.Http;
using RedBook.Core.Constants;
using System.Security.Claims;

namespace RedBook.Core.Models
{
    public class RequestingUser
    {
        private readonly HttpContext _httpContext;
        private readonly ClaimsPrincipal _contextReference;

        internal RequestingUser(ClaimsPrincipal contextReference, HttpContext httpContext)
        {
            _httpContext = httpContext;
            _contextReference = contextReference;
        }

        public int UserId
        {
            get
            {
                string? _userId = _contextReference.Claims.FirstOrDefault(x => x.Type.Equals("UserId", StringComparison.InvariantCultureIgnoreCase))?.Value;
                if (!string.IsNullOrWhiteSpace(_userId) && int.TryParse(_userId, out int userId))
                    return userId;
                else
                    throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidToken);
            }
        }

        public string RequestOrigin
        {
            get
            {
                return _httpContext.Request.Headers["Origin"].ToString();
            }
        }

        //public int[] RoleIds
        //{
        //    get
        //    {
        //        string? roleIds = _contextReference.Claims.FirstOrDefault(x => x.Type.Equals("UserRoleIds", StringComparison.InvariantCultureIgnoreCase))?.Value;
        //        if (!string.IsNullOrEmpty(roleIds))
        //        {
        //            string[] strArray = roleIds.Split(',');
        //            return Array.ConvertAll(strArray, int.Parse);
        //        }
        //        else
        //            throw new ArgumentException(CommonConstants.HttpResponseMessages.InvalidToken);
        //    }
        //}
    }
}
