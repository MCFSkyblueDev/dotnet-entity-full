using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class SpecificationEvaluator<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
        {
            var query = inputQuery;
            if(spec.Criteria != null) {
                query = query.Where(spec.Criteria);
            }
            if(spec.OrderBy != null) {
                Console.WriteLine("Order by: " + spec.OrderBy);
                query = query.OrderBy(spec.OrderBy);
            }
            if(spec.OrderByDescending != null) {
                Console.WriteLine("Order DESC by: " + spec.OrderBy);
                query = query.OrderByDescending(spec.OrderByDescending);
            }
            if(spec.IsPagingEnabled) {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }
            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));
            return query;
        }
    }
}