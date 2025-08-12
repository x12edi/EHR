using System;
using System.Linq;
using System.Linq.Expressions;

namespace EHR.Infrastructure.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> source, string orderByProperty, bool ascending)
        {
            if (string.IsNullOrWhiteSpace(orderByProperty))
                return source; // No sorting if empty

            var entityType = typeof(T);
            var property = entityType.GetProperty(orderByProperty,
                System.Reflection.BindingFlags.IgnoreCase |
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.Instance);

            if (property == null)
                return source; // Invalid property

            var parameter = Expression.Parameter(entityType, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);

            string orderByMethod = ascending ? "OrderBy" : "OrderByDescending";

            var resultExpression = Expression.Call(
                typeof(Queryable),
                orderByMethod,
                new Type[] { entityType, property.PropertyType },
                source.Expression,
                Expression.Quote(orderByExpression));

            return source.Provider.CreateQuery<T>(resultExpression);
        }
    }
}
