using System;

namespace Zoey.Quartz.Domain.Model
{
    public class Log
    {
        /// <summary>
        /// 执行时间
        /// </summary>
        public DateTime ExecutionTime { get; set; }
        /// <summary>
        /// IP
        /// </summary>
        public string ClientIpAddress { get; set; }
        /// <summary>
        /// 浏览器信息
        /// </summary>
        public string BrowserInfo { get; set; }
        /// <summary>
        /// 用户信息
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
    }
}
