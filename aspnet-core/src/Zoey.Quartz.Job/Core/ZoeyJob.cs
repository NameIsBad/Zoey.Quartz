using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;
using System;
using System.Threading.Tasks;

namespace Zoey.Quartz.Job
{
    /// <summary>
    /// A JobFactory is responsible for producing instances of Quartz.IJob classes.
    /// </summary>
    public class JobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public JobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            IJobDetail jobDetail = bundle.JobDetail;
            IJob job = (IJob)_serviceProvider.GetService(jobDetail.JobType);
            return job;
        }

        public void ReturnJob(IJob job)
        {
            IDisposable disposable = job as IDisposable;
            disposable?.Dispose();
        }
    }

    /// <summary>
    /// Job基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ZoeyJob<T> : IZoeyJob<T> where T : ZoeyJobDataMap
    {
        protected ZoeyJob()
        {
        }

        public virtual T GetJobData(IJobExecutionContext context)
        {
            T data = context.JobDetail.JobDataMap as T;
            return data;
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
        }

        public abstract Task Execute(IJobExecutionContext context);
    }

    public interface IZoeyJob<out T> : IJob where T : ZoeyJobDataMap
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="services"></param>
        void ConfigureServices(IServiceCollection services);

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        T GetJobData(IJobExecutionContext context);
    }
}
