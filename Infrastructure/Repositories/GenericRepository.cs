using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Repositories
{
    public class GenericRepository<T>(ApplicationDbContext context) : IGenericRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<T> CreateAsync(T value)
        {
            await _context.Set<T>().AddAsync(value);
            await _context.SaveChangesAsync();
            return value;
        }

        public async Task<T?> DeleteAsync(int id)
        {
            var value = await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
            if (value == null) return null;
            _context.Set<T>().Remove(value);
            await _context.SaveChangesAsync();
            return value;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToArrayAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> GetEntitiesWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<T?> GetEntityWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public async Task<T?> UpdateAsync(int id, object value)
        {
            var entity = await _context.Set<T>().FirstOrDefaultAsync(s => s.Id == id);
            if (entity == null) return null;
            var entry = _context.Entry(entity);

            var valueProperties = value.GetType().GetProperties();

            foreach (var prop in valueProperties)
            {
                var newValue = prop.GetValue(value);
                if (newValue != null && (newValue is not string str || !string.IsNullOrWhiteSpace(str)))
                {
                    var entityProperty = entry.Property(prop.Name);
                    if (entityProperty != null)
                    {
                        entityProperty.CurrentValue = newValue;
                        entityProperty.IsModified = true;
                    }
                }
            }

            entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
            entry.Property("UpdatedAt").IsModified = true;

            await _context.SaveChangesAsync();
            await _context.Entry(entity).ReloadAsync();
            return entity;
        }


        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }


        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>(), spec);
        }
    }
}