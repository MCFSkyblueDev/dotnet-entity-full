using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Specifications;

namespace Core.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IReadOnlyList<T>> GetAllAsync();

        Task<T?> GetByIdAsync(int id);

        Task<T> CreateAsync(T value);

        Task<T?> UpdateAsync(int id, object value);

        Task<T?> DeleteAsync(int id);

        Task<IReadOnlyList<T>> GetEntitiesWithSpecAsync(ISpecification<T> spec);

        Task<T?> GetEntityWithSpecAsync(ISpecification<T> spec);

        Task<int> CountAsync(ISpecification<T> spec);

    }
}