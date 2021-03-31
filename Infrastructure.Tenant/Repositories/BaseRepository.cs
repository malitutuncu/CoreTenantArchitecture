using Core.Global.Interfaces;
using Infrastructure.Tenant.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        private readonly TenantDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(TenantDbContext Context)
        {
            _context = Context;
            _dbSet = Context.Set<T>();
        }

        public virtual T GetById<TPrimaryKey>(TPrimaryKey id)
        {
            return _dbSet.Find(id);
        }

        public T Get(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.SingleOrDefault(predicate);
        }

        public IReadOnlyList<T> GetList(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null)
            {
                return _dbSet.ToList();
            }

            return _dbSet.Where(predicate).ToList();
        }

        public IReadOnlyList<T> GetPagedList(int pageNumber, int pageSize, Expression<Func<T, bool>> predicate)
        {
            if (predicate == null)
            {
                return _dbSet
                     .Skip((pageNumber - 1) * pageSize)
                     .Take(pageSize)
                     .AsNoTracking()
                     .ToList();
            }

            return _dbSet
                 .Where(predicate)
                 .Skip((pageNumber - 1) * pageSize)
                 .Take(pageSize)
                 .AsNoTracking()
                 .ToList();
        }

        public T Add(T entity)
        {
            _dbSet.Add(entity);
            return entity;
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public IEnumerable<T> AddRange(IEnumerable<T> entities)
        {
            _dbSet.AddRangeAsync(entities);
            return entities;
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }
    }
}