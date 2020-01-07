using Quartz;

namespace Zoey.Quartz.Core.Quartz
{
    /// <summary>
    /// 构建Job参数
    /// </summary>
    public class JobBuilderOption
    {
    }

    /// <summary>
    /// 构建Trigger参数
    /// </summary>
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
