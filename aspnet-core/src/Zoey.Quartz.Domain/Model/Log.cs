using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zoey.Quartz.Domain.Model
{
    public class Log
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
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
        [MaxLength(150)]
        public string BrowserInfo { get; set; }
        /// <summary>
        /// 用户信息
        /// </summary>
        [MaxLength(50)]
        public string UserName { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        [MaxLength(1500)]
        public string Content { get; set; }
    }
}
