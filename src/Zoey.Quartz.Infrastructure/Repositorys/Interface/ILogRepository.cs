using Zoey.Quartz.Core.Domain.Repositories;
using Zoey.Quartz.Domain.Model;
using System.Threading.Tasks;

namespace Zoey.Quartz.Infrastructure.Repositorys
{
    public interface ILogRepository : IRepository
    {
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="log"></param>
        Task WriteLog(Log log);
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="content"></param>
        Task WriteLog(string content);
    }
}
