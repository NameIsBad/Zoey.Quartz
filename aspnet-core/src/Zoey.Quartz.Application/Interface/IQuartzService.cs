using Zoey.Quartz.Core.Application.Services;
using Zoey.Quartz.Core.Web;
using Zoey.Quartz.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zoey.Quartz.Core.PageHelper;
using Zoey.Quartz.Domain.Dto.Request;

namespace Zoey.Quartz.Application
{
    public interface IQuartzService : IApplicationService
    {
        /// <summary>
        /// 从数据库中获取job并启动
        /// </summary>
        /// <returns></returns>
        public Task StartQuartz();
        /// <summary>
        /// 获取job
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public Task<PagingQueryResult<TaskOptions>> GetJobs(TaskOptionsQueryFilter queryFilter);
        /// <summary>
        /// 添加作业
        /// </summary>
        /// <param name="taskOptions"></param>
        /// <returns></returns>
        Task<AjaxResponse> AddJob(TaskOptions taskOptions);
        /// <summary>
        /// 移除作业
        /// </summary>
        /// <param name="taskOptions"></param>
        /// <returns></returns>
        Task<AjaxResponse> Remove(TaskOptions taskOptions);
        /// <summary>
        /// 更新作业
        /// </summary>
        /// <param name="taskOptions"></param>
        /// <returns></returns>
        Task<AjaxResponse> Update(TaskOptions taskOptions);
        /// <summary>
        /// 暂停作业
        /// </summary>
        /// <param name="taskOptions"></param>
        /// <returns></returns>
        Task<AjaxResponse> Pause(TaskOptions taskOptions);
        /// <summary>
        /// 启动作业
        /// </summary>
        /// <param name="taskOptions"></param>
        /// <returns></returns>
        Task<AjaxResponse> Start(TaskOptions taskOptions);
        /// <summary>
        /// 立即执行一次作业
        /// </summary>
        /// <param name="taskOptions"></param>
        /// <returns></returns>
        Task<AjaxResponse> Run(TaskOptions taskOptions);
    }
}
