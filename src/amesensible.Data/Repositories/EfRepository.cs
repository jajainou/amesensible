using amesensible.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace amesensible.Data.Repositories
{
    public class EfRepository<T> : IRepository<T> where T : class
    {
        protected AmeSensibleContext Context;

        public EfRepository(AmeSensibleContext context)
        {
            Context = context;
        }

        public T GetById(int id) => Context.Set<T>().Find(id);

        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
            => Context.Set<T>().FirstOrDefault(predicate);

        public T FirstOrDefault(Expression<Func<T, bool>> predicate, params string[] navigationProperties)
        {
            return GetWhere(predicate).FirstOrDefault();
        }

        public T FirstOrDefault<TProperty>(Expression<Func<T, bool>> predicate, params Expression<Func<T, TProperty>>[] navigationPropertyPaths)
        {
            if (navigationPropertyPaths == null)
                return Context.Set<T>().FirstOrDefault(predicate);
            var query = Context.Set<T>().AsQueryable();
            foreach (var navigationPropertyPath in navigationPropertyPaths)
            {
                query = query.Include(navigationPropertyPath);
            }
            return query.FirstOrDefault(predicate);
        }

        public void Add(T entity)
        {
            //  Context.Add(entity);
            Context.Set<T>().Add(entity);
            Context.SaveChanges();
        }

        public void Update(T entity, bool modifiedState = false)
        {
            if (modifiedState) Context.Entry(entity).State = EntityState.Modified;
            else Context.Set<T>().Update(entity);
            Context.SaveChanges();
        }

        public void Remove(T entity)
        {
            Context.Set<T>().Remove(entity);
            Context.SaveChanges();
        }

        public IQueryable<T> GetAll()
        {
            return Context.Set<T>();
        }

        public IQueryable<T> GetAll(params string[] navigationProperties)
        {
            var query = Context.Set<T>().AsQueryable();
            if (navigationProperties != null)
            {
                foreach (var navigationPropertiy in navigationProperties)
                {
                    query = query.Include(navigationPropertiy);
                }
            }
            return query;
        }

        public IQueryable<T> GetWhere(Expression<Func<T, bool>> predicate)
        {
            return Context.Set<T>().Where(predicate);
        }

        public IQueryable<T> GetWhere(Expression<Func<T, bool>> predicate, params string[] navigationProperties)
        {
            var query = Context.Set<T>().Where(predicate).AsQueryable();
            if (navigationProperties != null)
            {
                foreach (var navigationPropertiy in navigationProperties)
                {
                    query = query.Include(navigationPropertiy);
                }
            }
            return query;
        }

        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return Context.Set<T>().Any(predicate);
        }

        public int CountAll() => Context.Set<T>().Count();

        public int CountWhere(Expression<Func<T, bool>> predicate)
            => Context.Set<T>().Count(predicate);

    }
}
