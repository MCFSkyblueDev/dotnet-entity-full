using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Entities;
using Core.Params;

namespace Core.Specifications
{
    public class EntitiesWithPaginationSpecification<T> : BaseSpecification<T> where T : BaseEntity
    {
        public EntitiesWithPaginationSpecification(EntitySpecParams entityParams) : base(e => true)
        {
            ApplyPaging(entityParams.PageSize * (entityParams.PageNumber - 1), entityParams.PageSize);

            // Sorting
            if (!string.IsNullOrEmpty(entityParams.SortBy))
            {
                var propertyInfo = typeof(T).GetProperty(entityParams.SortBy);
                if (propertyInfo != null)
                {
                    // Dynamically create sorting expression
                    var param = Expression.Parameter(typeof(T), "p");
                    var property = Expression.Property(param, propertyInfo);
                    var lambda = Expression.Lambda<Func<T, object>>(Expression.Convert(property, typeof(object)), param);
                    if (entityParams.SortOrder?.ToLower() == "asc")
                    {
                        Console.WriteLine(lambda);
                        AddOrderBy(lambda);
                    }
                    else
                    {
                        AddOrderByDescending(lambda);
                    }
                }
            }
            else
            {
                AddOrderBy(p => p.Id);
            }
        }
    }
}