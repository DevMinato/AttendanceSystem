using System;
using System.Linq.Expressions;

namespace AttendanceSystem.Application.Helpers
{
    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> True<T>()
        {
            return Expression.Lambda<Func<T, bool>>(Expression.Constant(true), Expression.Parameter(typeof(T)));
        }

        public static Expression<Func<T, bool>> False<T>()
        {
            return Expression.Lambda<Func<T, bool>>(Expression.Constant(false), Expression.Parameter(typeof(T)));
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            var parameter = left.Parameters[0];
            var visitor = new SubstituteParameterVisitor(parameter);
            var body = Expression.OrElse(visitor.Visit(left.Body), visitor.Visit(right.Body));
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            var parameter = left.Parameters[0];
            var visitor = new SubstituteParameterVisitor(parameter);
            var body = Expression.AndAlso(visitor.Visit(left.Body), visitor.Visit(right.Body));
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        private class SubstituteParameterVisitor : ExpressionVisitor
        {
            private readonly ParameterExpression _parameter;

            public SubstituteParameterVisitor(ParameterExpression parameter)
            {
                _parameter = parameter;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return _parameter;
            }
        }
    }
}

