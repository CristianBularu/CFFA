using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using static CFFA_API.Logic.Helpers.LinqPaginationExtension;

namespace CFFA_API.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext Context;
        public Repository(DbContext context)
        {
            Context = context;
        }
        public void Add(T entity)
        {
            Context.Set<T>().Add(entity);
            Context.SaveChanges();
        }
        [Obsolete]
        public T Get(long Id)
        {
            return Context.Set<T>().Find(Id);
        }
        public IQueryable<T> GetAll(int pageIndex, int pageSize)
        {
            return Context.Set<T>().Page(pageIndex, pageSize).AsNoTracking();
        }
        public IQueryable<T> Find(Expression<Func<T, bool>> predicate, int pageIndex, int pageSize)
        {
            return Context.Set<T>().Where(predicate).Page(pageIndex, pageSize).AsNoTracking();
        }

        public virtual bool Update(T entity) { return false; }

        public void Remove(T entity)
        {
            Context.Set<T>().Remove(entity);
            Context.SaveChanges();
        }
        public void RemoveAll()
        {
            Context.Set<T>().RemoveRange(Context.Set<T>());
            Context.SaveChanges();
        }
    }
}
