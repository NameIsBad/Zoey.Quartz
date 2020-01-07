using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Zoey.Quartz.Core.PageHelper
{
    /// <summary>
    /// 分页查询结果
    /// </summary>
    [Serializable]
    public class PagingQueryResult<T>
        where T : class
    {
        public int TotalCount { get; set; }

        public int TotalPages { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public List<T> DataList { get; set; }

    }

    public static class PageLinqExtensions
    {
        public static PagingQueryResult<T> ToPageList<T>(this IQueryable<T> source, int pageIndex, int pageSize) where T : class
        {
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }

            int itemIndex = (pageIndex - 1) * pageSize;
            List<T> data = source.Skip(itemIndex).Take(pageSize).ToList();
            int count = source.Count();
            PagingQueryResult<T> result = new PagingQueryResult<T>
            {
                DataList = data,
                TotalCount = count,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            result.TotalPages = result.TotalCount % pageSize == 0
                ? result.TotalCount / pageSize
                : result.TotalCount / pageSize + 1;
            return result;
        }

        public static async Task<PagingQueryResult<T>> ToPageListAnync<T>(this IQueryable<T> source, int pageIndex, int pageSize) where T : class
        {
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }

            int itemIndex = (pageIndex - 1) * pageSize;
            int count = await source.CountAsync();
            List<T> data = new List<T>();
            if (count > 0)
                data = await source.Skip(itemIndex).Take(pageSize).ToListAsync();

            PagingQueryResult<T> result = new PagingQueryResult<T>
            {
                DataList = data,
                TotalCount = count,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            result.TotalPages = result.TotalCount % pageSize == 0
                ? result.TotalCount / pageSize
                : result.TotalCount / pageSize + 1;
            return result;
        }

        public static PagingQueryResult<T> ToPageList<T>(this IQueryable<T> source, PagingQueryFilter pageQueryFilter) where T : class
        {
            if (pageQueryFilter.PageIndex < 1)
                pageQueryFilter.PageIndex = 1;

            int itemIndex = (pageQueryFilter.PageIndex - 1) * pageQueryFilter.PageSize;
            int count = source.Count();
            List<T> data = new List<T>();
            if (count > 0)
                data = source.Skip(itemIndex).Take(pageQueryFilter.PageSize).ToList();
            PagingQueryResult<T> result = new PagingQueryResult<T>
            {
                DataList = data,
                TotalCount = count,
                PageIndex = pageQueryFilter.PageIndex,
                PageSize = pageQueryFilter.PageSize
            };
            result.TotalPages = result.TotalCount % pageQueryFilter.PageSize == 0
                ? result.TotalCount / pageQueryFilter.PageSize
                : result.TotalCount / pageQueryFilter.PageSize + 1;
            return result;
        }

        public static async Task<PagingQueryResult<T>> ToPageListAnync<T>(this IQueryable<T> source, PagingQueryFilter pageQueryFilter) where T : class
        {
            if (pageQueryFilter.PageIndex < 1)
                pageQueryFilter.PageIndex = 1;

            int itemIndex = (pageQueryFilter.PageIndex - 1) * pageQueryFilter.PageSize;
            if (!string.IsNullOrWhiteSpace(pageQueryFilter.SortOrder))
                source = source.OrderBy(pageQueryFilter.SortField, pageQueryFilter.SortOrder);
            int count = await source.CountAsync();
            List<T> data = new List<T>();
            if (count > 0)
                data = await source.Skip(itemIndex).Take(pageQueryFilter.PageSize).ToListAsync();
            PagingQueryResult<T> result = new PagingQueryResult<T>
            {
                DataList = data,
                TotalCount = count,
                PageIndex = pageQueryFilter.PageIndex,
                PageSize = pageQueryFilter.PageSize
            };
            result.TotalPages = result.TotalCount % pageQueryFilter.PageSize == 0
                ? result.TotalCount / pageQueryFilter.PageSize
                : result.TotalCount / pageQueryFilter.PageSize + 1;
            return result;
        }
    }
}