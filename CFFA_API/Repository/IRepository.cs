using System;
using System.Linq;
using System.Linq.Expressions;

namespace CFFA_API.Repository
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        T Get(long Id);
        bool Update(T entity);
        void Remove(T entity);
        void RemoveAll();
        IQueryable<T> GetAll(int pageIndex, int pageSize);
        IQueryable<T> Find(Expression<Func<T, bool>> predicate, int pageIndex, int pageSize);
    }
}
