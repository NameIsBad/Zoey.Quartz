using Zoey.Quartz.Core.Quartz;
using Quartz;

namespace Zoey.Quartz.Job
{
    public class JobManager : IJobManager
    {
        public JobBuilder GetJob(JobBuilderOption option = null)
        {
            JobBuilder jobBuilder;
            if (option == null)
                jobBuilder = JobBuilder.Create<HttpJob>();
            else
                jobBuilder = JobBuilder.Create<HttpJob>();
            return jobBuilder;
        }
    }

    public class TriggerManager : ITriggerManager
    {
        public TriggerBuilder Create(TriggerBuilderOption option = null)
        {
            TriggerBuilder triggerBuilder;
            if (option == null)
                triggerBuilder = TriggerBuilder.Create();
            else
                triggerBuilder = TriggerBuilder.Create();
            return triggerBuilder;
        }
    }
}
