using System;
using System.Collections.Generic;
using System.Text;
using Zoey.Quartz.Core.PageHelper;

namespace Zoey.Quartz.Domain.Dto.Request
{
    public class TaskOptionsQueryFilter : PagingQueryFilter
    {
        /// <summary>
        /// Task名称
        /// </summary>
        public string TaskName { get; set; }
    }
}
