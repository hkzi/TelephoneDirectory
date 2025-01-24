using Castle.DynamicProxy;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;
using TelephoneDirectory.Business.Constants;
using TelephoneDirectory.Core.Entities.Concrete;
using TelephoneDirectory.Core.Extensions;
using TelephoneDirectory.Core.Utilities.Interceptors;
using TelephoneDirectory.Core.Utilities.IoC;


namespace TelephoneDirectory.Business.BusinessAspects.Autofac
{
   
    public class SecuredOperation : MethodInterception,IFilterMetadata
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string[] _roles;

        

        public SecuredOperation(string roles)
        {
            _roles = roles.Split(',');
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();

        }

        protected override void OnBefore(IInvocation invocation)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null || httpContext.User == null || !httpContext.User.Identity.IsAuthenticated)
            {
                throw new Exception("User is not authenticated.");
            }

            var roleClaims = GetUserRoles(httpContext.User);
            if (roleClaims == null || !roleClaims.Any())
            {
                throw new Exception("User does not have any roles assigned.");
            }

            if (!_roles.Any(role => roleClaims.Contains(role)))
            {
                throw new Exception("You are not authorized!");
            }
        }

        private List<string> GetUserRoles(ClaimsPrincipal user)
        {
            return user?.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList() ?? new List<string>();
        }

    }
}