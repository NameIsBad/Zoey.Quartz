using Quartz;

namespace Zoey.Quartz.Core.Quartz
{
    public class JobBuilderOption
    {
    }
    public class TriggerBuilderOption
    {
    }

    /// <summary>
    /// Job管理器
    /// </summary>
    public interface IJobManager
    {
        JobBuilder GetJob(JobBuilderOption option = null);
    }

    /// <summary>
    /// Trigger管理器
    /// </summary>
    public interface ITriggerManager
    {
        TriggerBuilder Create(TriggerBuilderOption option = null);
    }
}
