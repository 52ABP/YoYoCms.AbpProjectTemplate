using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using LTM.Common.Exceptions;
using LTM.Common.Extensions;
using LTM.Common.Properties;

namespace LTM.Common.Filter
{
    /// <summary>
    ///     查询表达式辅助操作类
    /// </summary>
    public static class FilterHelper
    {
        #region 字段

        private static readonly Dictionary<FilterOperate, Func<Expression, Expression, Expression>> ExpressionDict =
            new Dictionary<FilterOperate, Func<Expression, Expression, Expression>>
            {
                {
                    FilterOperate.Equal, Expression.Equal
                },
                {
                    FilterOperate.NotEqual, Expression.NotEqual
                },
                {
                    FilterOperate.Less, Expression.LessThan
                },
                {
                    FilterOperate.Greater, Expression.GreaterThan
                },
                {
                    FilterOperate.LessOrEqual, Expression.LessThanOrEqual
                },
                {
                    FilterOperate.GreaterOrEqual, Expression.GreaterThanOrEqual
                },
                {
                    FilterOperate.StartsWith,
                    (left, right) =>
                    {
                        if (left.Type != typeof (string))
                        {
                            throw new NotSupportedException("“StartsWith”比较方式只支持字符串类型的数据");
                        }
                        return Expression.Call(left, typeof (string).GetMethod("StartsWith", new[] {typeof (string)}),
                            right);
                    }
                },
                {
                    FilterOperate.EndsWith,
                    (left, right) =>
                    {
                        if (left.Type != typeof (string))
                        {
                            throw new NotSupportedException("“EndsWith”比较方式只支持字符串类型的数据");
                        }
                        return Expression.Call(left, typeof (string).GetMethod("EndsWith", new[] {typeof (string)}),
                            right);
                    }
                },
                {
                    FilterOperate.Contains,
                    (left, right) =>
                    {
                        if (left.Type != typeof (string))
                        {
                            throw new NotSupportedException("“Contains”比较方式只支持字符串类型的数据");
                        }
                        return Expression.Call(left, typeof (string).GetMethod("Contains", new[] {typeof (string)}),
                            right);
                    }
                }
                //{
                //    FilterOperates.StdIn, (left, right) =>
                //    {
                //        if (!right.Type.IsArray)
                //        {
                //            return null;
                //        }
                //        return left.Type != typeof (string) ? null : Expression.Call(typeof (Enumerable), "Contains", new[] {left.Type}, right, left);
                //    }
                //},
                //{
                //    FilterOperates.DataTimeLessThanOrEqual, Expression.LessThanOrEqual
                //}
            };

        #endregion 字段

        /// <summary>
        ///     获取指定查询条件组的查询表达式
        /// </summary>
        /// <typeparam name="T">表达式实体类型</typeparam>
        /// <param name="group">查询条件组，如果为null，则直接返回 true 表达式</param>
        public static Expression<Func<T, bool>> GetExpression<T>(FilterGroup group)
        {
            var param = Expression.Parameter(typeof (T), "m");
            var body = GetExpressionBody(param, group);
            var expression = Expression.Lambda<Func<T, bool>>(body, param);
            return expression;
        }

        /// <summary>
        ///     获取指定查询条件的查询表达式
        /// </summary>
        /// <typeparam name="T">表达式实体类型</typeparam>
        /// <param name="rule">查询条件，如果为null，则直接返回 true 表达式</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> GetExpression<T>(FilterRule rule = null)
        {
            var param = Expression.Parameter(typeof (T), "m");
            var body = GetExpressionBody(param, rule);
            var expression = Expression.Lambda<Func<T, bool>>(body, param);
            return expression;
        }

        /// <summary>
        ///     把查询操作的枚举表示转换为操作码
        /// </summary>
        /// <param name="operate">查询操作的枚举表示</param>
        public static string ToOperateCode(this FilterOperate operate)
        {
            var type = operate.GetType();
            var members = type.GetMember(operate.CastTo<string>());
            if (members.Length > 0)
            {
                var attributes = members[0].GetCustomAttributes(typeof (OperateCodeAttribute), false);
                if (attributes.Length > 0)
                {
                    return ((OperateCodeAttribute) attributes[0]).Code;
                }
            }
            return null;
        }

        /// <summary>
        ///     把查询操作的枚举表示转换为操作名称
        /// </summary>
        /// <param name="operate">查询操作的枚举表示</param>
        /// <returns></returns>
        public static string ToOperateName(this FilterOperate operate)
        {
            var type = operate.GetType();
            var members = type.GetMember(operate.CastTo<string>());
            if (members.Length > 0)
            {
                var attributes = members[0].GetCustomAttributes(typeof (OperateCodeAttribute), false);
                if (attributes.Length > 0)
                {
                    return ((OperateCodeAttribute) attributes[0]).Name;
                }
            }
            return null;
        }

        /// <summary>
        ///     获取操作码的查询操作枚举表示
        /// </summary>
        /// <param name="code">操作码</param>
        /// <returns></returns>
        public static FilterOperate GetFilterOperate(string code)
        {
            var type = typeof (FilterOperate);
            var members = type.GetMembers(BindingFlags.Public | BindingFlags.Static);
            foreach (var member in members)
            {
                var operate = member.Name.CastTo<FilterOperate>();
                if (operate.ToOperateCode() == code)
                {
                    return operate;
                }
            }
            throw new NotSupportedException("获取操作码的查询操作枚举表示时不支持代码：" + code);
        }

        #region 私有方法

        private static Expression GetExpressionBody(ParameterExpression param, FilterGroup group)
        {
            param.CheckNotNull(nameof(param));

            //如果无条件或条件为空，直接返回 true表达式
            if (group == null || (group.Rules.Count == 0 && group.Groups.Count == 0))
            {
                return Expression.Constant(true);
            }
            var bodys = new List<Expression>();
            bodys.AddRange(group.Rules.Select(rule => GetExpressionBody(param, rule)));
            bodys.AddRange(group.Groups.Select(subGroup => GetExpressionBody(param, subGroup)));

            if (group.Operate == FilterOperate.And)
            {
                return bodys.Aggregate(Expression.AndAlso);
            }
            if (group.Operate == FilterOperate.Or)
            {
                return bodys.Aggregate(Expression.OrElse);
            }
            throw new KingsSharpException(Resources.Filter_GroupOperateError);
        }

        private static Expression GetExpressionBody(ParameterExpression param, FilterRule rule)
        {
            if (rule == null || rule.Value == null || string.IsNullOrEmpty(rule.Value.ToString()))
            {
                return Expression.Constant(true);
            }
            var expression = GetPropertyLambdaExpression(param, rule);
            var constant = ChangeTypeToExpression(rule, expression.Body.Type);
            return ExpressionDict[rule.Operate](expression.Body, constant);
        }

        private static LambdaExpression GetPropertyLambdaExpression(ParameterExpression param, FilterRule rule)
        {
            var propertyNames = rule.Field.Split('.');
            Expression propertyAccess = param;
            var type = param.Type;
            foreach (var propertyName in propertyNames)
            {
                var property = type.GetProperty(propertyName);
                if (property == null)
                {
                    throw new KingsSharpException(string.Format(Resources.Filter_RuleFieldInTypeNotFound, rule.Field,
                        type.FullName));
                }
                type = property.PropertyType;
                propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
            }
            return Expression.Lambda(propertyAccess, param);
        }

        private static Expression ChangeTypeToExpression(FilterRule rule, Type conversionType)
        {
            //if (item.Method == QueryMethod.StdIn)
            //{
            //    Array array = (item.Value as Array);
            //    List<Expression> expressionList = new List<Expression>();
            //    if (array != null)
            //    {
            //        expressionList.AddRange(array.Cast<object>().Select((t, i) =>
            //            ChangeType(array.GetValue(i), conversionType)).Select(newValue => Expression.Constant(newValue, conversionType)));
            //    }
            //    return Expression.NewArrayInit(conversionType, expressionList);
            //}

            var elementType = conversionType.GetUnNullableType();
            var value = rule.Value is string
                ? rule.Value.ToString().CastTo(conversionType)
                : Convert.ChangeType(rule.Value, elementType);
            return Expression.Constant(value, conversionType);
        }

        #endregion 私有方法
    }
}