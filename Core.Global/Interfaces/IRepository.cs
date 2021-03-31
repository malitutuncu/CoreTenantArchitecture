using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Global.Interfaces
{
    public interface IRepository<T> where T : class
    {
        T GetById<TPrimaryKey>(TPrimaryKey id);
        T Get(Expression<Func<T, bool>> predicate);
        IReadOnlyList<T> GetList(Expression<Func<T, bool>> predicate);
        IReadOnlyList<T> GetPagedList(int pageNumber, int pageSize, Expression<Func<T, bool>> predicate);
        T Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        IEnumerable<T> AddRange(IEnumerable<T> entities);
        void DeleteRange(IEnumerable<T> entities);
    }
}
