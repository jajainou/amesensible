using System;
using System.Linq;
using System.Linq.Expressions;

namespace amesensible.Core.Interfaces
{
    public interface IRepository<T> where T : class
    {
        T GetById(int id);
        T FirstOrDefault(Expression<Func<T, bool>> predicate);
        T FirstOrDefault(Expression<Func<T, bool>> predicate, params string[] navigationProperties);
        T FirstOrDefault<TProperty>(Expression<Func<T, bool>> predicate, params Expression<Func<T, TProperty>>[] navigationPropertyPaths);

        void Add(T entity);
        void Update(T entity, bool modifiedState = false);
        void Remove(T entity);

        IQueryable<T> GetAll();
        IQueryable<T> GetAll(params string[] navigationProperties);
        IQueryable<T> GetWhere(Expression<Func<T, bool>> predicate);
        IQueryable<T> GetWhere(Expression<Func<T, bool>> predicate, params string[] navigationProperties);
        bool Any(Expression<Func<T, bool>> predicate);
        int CountAll();
        int CountWhere(Expression<Func<T, bool>> predicate);

    }
}
