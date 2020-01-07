using Zoey.Quartz.Application;
using Zoey.Quartz.Domain.Model;
using Zoey.Quartz.Web.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Zoey.Quartz.Domain.Dto.Request;

namespace Zoey.Quartz.Web.Controllers
{
    public class TaskBackGroundController : Controller
    {
        private readonly IQuartzService _quartzService;
        private readonly ITaskLogService _quartzLog;
        public TaskBackGroundController(IQuartzService quartzCore, ITaskLogService quartzLogService)
        {
            _quartzLog = quartzLogService;
            _quartzService = quartzCore;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View("~/Views/TaskBackGround/Index.cshtml");
        }

        /// <summary>
        /// 获取所有的作业
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GetJobs(TaskOptionsQueryFilter queryFilter)
        {
            return Json(await _quartzService.GetJobs(queryFilter));
        }
        /// <summary>
        /// 获取作业运行日志
        /// </summary>
        /// <param name="taskName"></param>
        /// <param name="groupName"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetRunLog(TaskLogQueryFilter queryFilter)
        {
            var result = await _quartzLog.GetJobRunLog(queryFilter);
            return Json(result);
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="taskOptions"></param>
        /// <returns></returns>
        [TaskAuthor]
        public async Task<IActionResult> Add(TaskOptions taskOptions)
        {
            return Json(await _quartzService.AddJob(taskOptions));
        }
        [TaskAuthor]
        public async Task<IActionResult> Remove(TaskOptions taskOptions)
        {
            return Json(await _quartzService.Remove(taskOptions));
        }
        [TaskAuthor]
        public async Task<IActionResult> Update(TaskOptions taskOptions)
        {
            return Json(await _quartzService.Update(taskOptions));
        }
        [TaskAuthor]
        public async Task<IActionResult> Pause(TaskOptions taskOptions)
        {
            return Json(await _quartzService.Pause(taskOptions));
        }
        [TaskAuthor]
        public async Task<IActionResult> Start(TaskOptions taskOptions)
        {
            return Json(await _quartzService.Start(taskOptions));
        }
        [TaskAuthor]
        public async Task<IActionResult> Run(TaskOptions taskOptions)
        {
            return Json(await _quartzService.Run(taskOptions));
        }
    }




}