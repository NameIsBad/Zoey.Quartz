using Zoey.Quartz.Application;
using Zoey.Quartz.Core;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Zoey.Quartz.Web.Worker
{
    public class DefaultWorker : BackgroundService
    {
        private readonly IQuartzService _quartzService;
        public DefaultWorker(IQuartzService quartzCore)
        {
            _quartzService = quartzCore;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return _quartzService.StartQuartz();
        }
    }
}
