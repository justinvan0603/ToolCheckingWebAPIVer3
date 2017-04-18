using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ChatBot.Model.Abstract;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ChatBot.Data.Infrastructure
{
    public interface IRepositoryBase<T> where T : class, new()
    {
        //  IEnumerable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);
        //  Task<IEnumerable<T>> AllIncludingAsync(params Expression<Func<T, object>>[] includeProperties);
        //   IEnumerable<T> GetAll();

        //   Task<IEnumerable<T>> GetAllAsync();

        IEnumerable<T> GetAll(string[] includes = null);
        T GetSingle(int id);
        T GetSingle(Expression<Func<T, bool>> predicate);
        T GetSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
       // Task<T> GetSingleAsync(int id);
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
        //   Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate);
        //  void Add(T entity);

        EntityEntry<T> Add(T entity);

        //void Delete(T entity);
        //void Delete(int id);
        EntityEntry<T> Delete(T entity);

        EntityEntry<T> Delete(int id);

        void Update(T entity);
        void Commit();

        IEnumerable<T> GetMulti(Expression<Func<T, bool>> predicate, string[] includes = null);

        int Count(Expression<Func<T, bool>> where);

        bool CheckContains(Expression<Func<T, bool>> predicate);

        T GetSingleByCondition(Expression<Func<T, bool>> expression, string[] includes = null);
        //Delete multi records
        void DeleteMulti(Expression<Func<T, bool>> where);

        T GetSingleById(int id);
    }
}
