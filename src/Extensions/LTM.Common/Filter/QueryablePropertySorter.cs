using System.Collections.Concurrent;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using LTM.Common.Exceptions;
using LTM.Common.Properties;

namespace LTM.Common.Filter
{
    /// <summary>
    ///     <see cref="IQueryable{T}" />类型字符串排序操作类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class QueryablePropertySorter<T>
    {
// ReSharper disable StaticFieldInGenericType
        private static readonly ConcurrentDictionary<string, LambdaExpression> Cache =
            new ConcurrentDictionary<string, LambdaExpression>();

        /// <summary>
        ///     按指定的属性名称对<see cref="IQueryable{T}" />序列进行排序
        /// </summary>
        /// <param name="source">IQueryable{T}序列</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="sortDirection">排序方向</param>
        /// <returns></returns>
        public static IOrderedQueryable<T> OrderBy(IQueryable<T> source, string propertyName,
            ListSortDirection sortDirection)
        {
            dynamic keySelector = GetKeySelector(propertyName);
            return sortDirection == ListSortDirection.Ascending
                ? Queryable.OrderBy(source, keySelector)
                : Queryable.OrderByDescending(source, keySelector);
        }

        /// <summary>
        ///     按指定的属性名称对<see cref="IOrderedQueryable{T}" />序列进行排序
        /// </summary>
        /// <param name="source">IOrderedQueryable{T}序列</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="sortDirection">排序方向</param>
        /// <returns></returns>
        public static IOrderedQueryable<T> ThenBy(IOrderedQueryable<T> source, string propertyName,
            ListSortDirection sortDirection)
        {
            dynamic keySelector = GetKeySelector(propertyName);
            return sortDirection == ListSortDirection.Ascending
                ? Queryable.ThenBy(source, keySelector)
                : Queryable.ThenByDescending(source, keySelector);
        }

        private static LambdaExpression GetKeySelector(string keyName)
        {
            var type = typeof (T);
            var key = type.FullName + "." + keyName;
            if (Cache.ContainsKey(key))
            {
                return Cache[key];
            }
            var param = Expression.Parameter(type);
            var propertyNames = keyName.Split('.');
            Expression propertyAccess = param;
            foreach (var propertyName in propertyNames)
            {
                var property = type.GetProperty(propertyName);
                if (property == null)
                {
                    throw new KingsSharpException(string.Format(Resources.ObjectExtensions_PropertyNameNotExistsInType,
                        propertyName));
                }
                type = property.PropertyType;
                propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
            }
            var keySelector = Expression.Lambda(propertyAccess, param);
            Cache[key] = keySelector;
            return keySelector;
        }
    }
}