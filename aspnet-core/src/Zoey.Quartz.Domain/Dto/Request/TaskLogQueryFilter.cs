using System.ComponentModel.DataAnnotations;
using Zoey.Quartz.Core.PageHelper;

namespace Zoey.Quartz.Domain.Dto.Request
{
    public class TaskLogQueryFilter : PagingQueryFilter
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        [Required(ErrorMessage ="任务ID不能为空")]
        public int TaskId { get; set; }
    }
}
