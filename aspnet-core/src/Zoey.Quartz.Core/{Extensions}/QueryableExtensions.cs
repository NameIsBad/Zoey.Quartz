using System.Reflection;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Some useful extension methods for <see cref="IQueryable{T}"/>.
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// 如果给定条件为真，则通过<paramref name="condition"/>筛选<see cref="IQueryable{T}"/>。
        /// </summary>
        /// <param name="source"></param>
        /// <param name="condition">条件</param>
        /// <param name="predicate">过滤条件</param>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
        {
            return condition
                ? query.Where(predicate)
                : query;
        }

        /// <summary>
        /// 如果给定条件为真，则通过<paramref name="condition"/>筛选<see cref="IQueryable{T}"/>。
        /// </summary>
        /// <param name="source"></param>
        /// <param name="condition">条件</param>
        /// <param name="predicate">过滤条件</param>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, int, bool>> predicate)
        {
            return condition
                ? query.Where(predicate)
                : query;
        }

        #region OrderBy
        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="Field"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, string Field, string orderby = "orderb")
        {
            if (!string.IsNullOrEmpty(Field))
            {
                ParameterExpression p = Expression.Parameter(typeof(T));
                Expression key = Expression.Property(p, Field);

                PropertyInfo propInfo = GetPropertyInfo(typeof(T), Field);
                LambdaExpression expr = GetOrderExpression(typeof(T), propInfo);

                if (!("desc" == orderby.ToLower()))
                {
                    MethodInfo method = typeof(Queryable).GetMethods().FirstOrDefault(m => m.Name == "OrderBy" && m.GetParameters().Length == 2);
                    MethodInfo genericMethod = method.MakeGenericMethod(typeof(T), propInfo.PropertyType);
                    return (IQueryable<T>)genericMethod.Invoke(null, new object[] { query, expr });
                }
                else
                {
                    MethodInfo method = typeof(Queryable).GetMethods().FirstOrDefault(m => m.Name == "OrderByDescending" && m.GetParameters().Length == 2);
                    MethodInfo genericMethod = method.MakeGenericMethod(typeof(T), propInfo.PropertyType);
                    return (IQueryable<T>)genericMethod.Invoke(null, new object[] { query, expr });
                }
            }
            return query;
        }
        /// <summary>
        /// 获取反射
        /// </summary>
        /// <param name="objType"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private static PropertyInfo GetPropertyInfo(Type objType, string name)
        {
            PropertyInfo[] properties = objType.GetProperties();
            PropertyInfo matchedProperty = properties.FirstOrDefault(p => p.Name.ToLower() == name.ToLower());
            if (matchedProperty == null)
            {
                throw new ArgumentException("未找到匹配的排序字段");
            }

            return matchedProperty;
        }
        /// <summary>
        /// 获取生成表达式
        /// </summary>
        /// <param name="objType"></param>
        /// <param name="pi"></param>
        /// <returns></returns>
        private static LambdaExpression GetOrderExpression(Type objType, PropertyInfo pi)
        {
            ParameterExpression paramExpr = Expression.Parameter(objType);
            MemberExpression propAccess = Expression.PropertyOrField(paramExpr, pi.Name);
            LambdaExpression expr = Expression.Lambda(propAccess, paramExpr);
            return expr;
        }
        #endregion
    }
}
