using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;
using System;
using System.Threading.Tasks;

namespace Zoey.Quartz.Core.Quartz
{
    /// <summary>
    /// A JobFactory is responsible for producing instances of Quartz.IJob classes.
    /// </summary>
    public class QuartzFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public QuartzFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            IJobDetail jobDetail = bundle.JobDetail;
            var job = (ZoeyQuartzJob)_serviceProvider.GetService(jobDetail.JobType);
            return job;
        }

        public void ReturnJob(IJob job)
        {
            IDisposable disposable = job as IDisposable;
            disposable?.Dispose();
        }
    }

    public abstract class ZoeyQuartzJob : IJob
    {
        public ZoeyQuartzJob()
        {
        }

        /// <summary>
        /// Job名称
        /// </summary>
        public abstract string JobName { get; }

        public virtual void ConfigureServices(IServiceCollection services)
        {

        }

        public abstract Task Execute(IJobExecutionContext context);
    }
}
