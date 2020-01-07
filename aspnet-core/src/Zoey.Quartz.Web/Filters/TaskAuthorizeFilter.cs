using Zoey.Quartz.Web.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Linq;
using System.Net;

namespace Zoey.Quartz.Web.Filters
{
    public class TaskAuthorizeFilter : IAuthorizationFilter
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IMemoryCache _memoryCache;
        public TaskAuthorizeFilter(IHttpContextAccessor accessor, IMemoryCache memoryCache)
        {
            _accessor = accessor;
            _memoryCache = memoryCache;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.Filters.Any(item => item is IAllowAnonymousFilter))
            {
                return;
            }

            if (((ControllerActionDescriptor)context.ActionDescriptor).MethodInfo
                .CustomAttributes.Any(x => x.AttributeType == typeof(TaskAuthorAttribute))
                && !_memoryCache.Get<bool>("isSuperToken"))
            {
                context.Result = new ContentResult()
                {
                    Content = JsonConvert.SerializeObject(new
                    {
                        status = false,
                        msg = "普通帐号不能进行此操作！可通过appsettings.json节点superToken获取管理员帐号。"
                    }),
                    ContentType = "application/json",
                    StatusCode = (int)HttpStatusCode.OK
                };
            }
        }
    }
}
