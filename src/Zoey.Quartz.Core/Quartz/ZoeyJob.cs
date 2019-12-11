using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;
using System;
using System.Threading.Tasks;

namespace Zoey.Quartz.Core.Quartz
{

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
            var job = (ZoeyJob)_serviceProvider.GetService(jobDetail.JobType);
            return job;
        }

        public void ReturnJob(IJob job)
        {
            IDisposable disposable = job as IDisposable;
            disposable?.Dispose();
        }
    }

    public abstract class ZoeyJob : IJob
    {
        public ZoeyJob()
        {
        }
        public virtual void ConfigureServices(IServiceCollection services)
        {

        }

        public abstract Task Execute(IJobExecutionContext context);
    }
}
