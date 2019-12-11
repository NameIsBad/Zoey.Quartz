using Zoey.Quartz.Application;
using Zoey.Quartz.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace Zoey.Quartz.Web.Controllers
{
    public class HealthController : Controller
    {
        private readonly ITaskLogService _quartzLog;

        public HealthController(ITaskLogService quartzLogService)
        {
            _quartzLog = quartzLogService;
        }
        /// <summary>
        /// 定时调用此接口让站点一直保持运行状态
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost, HttpGet]
        public IActionResult KeepAlive()
        {
            return Json(new { status = true });
        }

        [AllowAnonymous]
        [HttpPost, HttpGet]
        public IActionResult Index()
        {
            return Json(new { status = true, msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
        }
    }
}