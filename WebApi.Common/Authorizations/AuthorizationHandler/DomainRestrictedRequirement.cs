using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Common.Authorizations.AuthorizationHandler
{

    /// <summary>
    /// 为授权处理程序提供自定义资源
    /// </summary>
    ///  HubInvocationContext包括 HubCallerContext 、正在调用的集线器方法的名称，以及中心方法的参数。
    public class DomainRestrictedRequirement : AuthorizationHandler<DomainRestrictedRequirement, HubInvocationContext>,
     IAuthorizationRequirement
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DomainRestrictedRequirement requirement, HubInvocationContext resource)
        {
            if (IsUserAllowedToDoThis(resource.HubMethodName, context.User.Identity.Name) &&
             context.User.Identity.Name.EndsWith("@microsoft.com"))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }

        private bool IsUserAllowedToDoThis(string hubMethodName,string currentUsername)
        {
            return !(currentUsername.Equals("asdf42@microsoft.com") &&
                hubMethodName.Equals("banUser", StringComparison.OrdinalIgnoreCase));
        }
    }
}
