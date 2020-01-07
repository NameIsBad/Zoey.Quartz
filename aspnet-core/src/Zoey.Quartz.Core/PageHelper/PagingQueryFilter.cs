using System;

namespace Zoey.Quartz.Core.PageHelper
{
    /// <summary>
    /// 带分页相关的查询参数
    /// </summary>
    /// <remarks>
    /// </remarks>
    [Serializable]
    public class PagingQueryFilter : BaseQueryFilter
    {
        public PagingQueryFilter()
            : this(1, 10)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="pageIndex">页码 从1开始</param>
        /// <param name="pageSize">页量</param>
        public PagingQueryFilter(int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
        }
        /// <summary>
        /// </summary>
        /// <param name="pageIndex">页码 从1开始</param>
        /// <param name="pageSize">页量</param>
        public PagingQueryFilter(int pageIndex)
        {
            PageIndex = pageIndex;
            PageSize = 10;
        }

        /// <summary>
        /// 起始行号(从1开始)
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 获取数量
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string SortField { get; set; }

        /// <summary>
        /// 排序方式
        /// </summary>
        public string SortOrder { get; set; }

        /// <summary>
        /// 为空获取所有
        /// true删除
        /// false未删除
        /// </summary>
        public bool? IsDeleted { get; set; }

        /// <summary>
        /// 返回排序
        ///  例如：ID DESC或者 ID
        /// </summary>
        public string Sort
        {
            get
            {
                if (string.IsNullOrEmpty(SortField))
                {
                    return string.Empty;
                }

                string sort = SortField.Trim();
                if (!string.IsNullOrEmpty(SortOrder))
                {
                    sort += " " + SortOrder.Trim();
                }

                return sort;
            }
        }
    }
}