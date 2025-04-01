using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Specifications
{
    public class EntityByUniqueProperty<T> : BaseSpecification<T> where T : BaseEntity
    {
        public EntityByUniqueProperty(string propertyName, object propertyValue)
          : base(GetPropertyEqualsExpression<T>(propertyName, propertyValue))
        {
        }

        private static Expression<Func<TEntity , bool>> GetPropertyEqualsExpression<TEntity>(string propertyName, object propertyValue)
        {
            //Check property is available
            var propertyInfo = typeof(TEntity).GetProperty(propertyName);
            if (propertyInfo == null)
            {
                return Expression.Lambda<Func<TEntity , bool>>(Expression.Constant(false), Expression.Parameter(typeof(TEntity ), "e"));
            }
            var param = Expression.Parameter(typeof(TEntity ), "e");
            var property = Expression.Property(param, propertyName);
            var value = Expression.Constant(propertyValue);
            // Create equality expression: e => e.Property == propertyValue
            var body = Expression.Equal(property, value);
            return Expression.Lambda<Func<TEntity , bool>>(body, param);
        }
    }

}