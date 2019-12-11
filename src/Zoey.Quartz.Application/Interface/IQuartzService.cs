using Zoey.Quartz.Core.Application.Services;
using Zoey.Quartz.Core.Web;
using Zoey.Quartz.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Zoey.Quartz.Application
{
    public interface IQuartzService : IApplicationService
    {
        public Task StartQuartz();
        public Task<IEnumerable<TaskOptions>> GetJobs();
        Task<AjaxResponse> AddJob(TaskOptions taskOptions, bool init = false);
        Task<AjaxResponse> Remove(TaskOptions taskOptions);
        Task<AjaxResponse> Update(TaskOptions taskOptions);
        Task<AjaxResponse> Pause(TaskOptions taskOptions);
        Task<AjaxResponse> Start(TaskOptions taskOptions);
        Task<AjaxResponse> Run(TaskOptions taskOptions);
    }
}
