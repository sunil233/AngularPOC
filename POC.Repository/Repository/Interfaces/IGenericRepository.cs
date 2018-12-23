using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace POCRepository.Repository
{
    /// <summary>
    /// IGenericRepository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> Find(Expression<Func<T, bool>> query);

        IEnumerable<T> GetAll();

        void Add(T entity);

        void Delete(T entity);

        void Update(T entity);

        T GetByID(int Id);
    }
}