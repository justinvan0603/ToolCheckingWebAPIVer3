using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using ChatBot.Model.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ChatBot.Data.Infrastructure
{
    public class RepositoryBase<T> : IRepositoryBase<T>
            where T : class, new()
    {

        private ChatBotDbContext _context;

        #region Properties
        public RepositoryBase(ChatBotDbContext context)
        {
            _context = context;
        }
        #endregion
        //public virtual IEnumerable<T> GetAll()
        //{
        //    return _context.Set<T>().AsEnumerable();
        //}

        public IEnumerable<T> GetAll(string[] includes = null)
        {
            //HANDLE INCLUDES FOR ASSOCIATED OBJECTS IF APPLICABLE
            if (includes != null && includes.Count() > 0)
            {
                var query = _context.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include);
                return query.AsQueryable();
            }

            return _context.Set<T>().AsQueryable();
        }
        protected ChatBotDbContext DbContext => _context;

        //public virtual async Task<IEnumerable<T>> GetAllAsync()
        //{
        //    return await _context.Set<T>().ToListAsync();
        //}
        //public virtual IEnumerable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
        //{
        //    IQueryable<T> query = _context.Set<T>();
        //    foreach (var includeProperty in includeProperties)
        //    {
        //        query = query.Include(includeProperty);
        //    }
        //    return query.AsEnumerable();
        //}

        //public virtual async Task<IEnumerable<T>> AllIncludingAsync(params Expression<Func<T, object>>[] includeProperties)
        //{
        //    IQueryable<T> query = _context.Set<T>();
        //    foreach (var includeProperty in includeProperties)
        //    {
        //        query = query.Include(includeProperty);
        //    }
        //    return await query.ToListAsync();
        //}
        public T GetSingle(int id)
        {
            return _context.Set<T>().Find(id);
            //       return _context.Set<T>().FirstOrDefault(x => x.Id == id);

        }

        public T GetSingle(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().FirstOrDefault(predicate);
        }

        public T GetSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query.Where(predicate).FirstOrDefault();
        }

        //public async Task<T> GetSingleAsync(int id)
        //{
        //    return await _context.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
        //}
        public virtual IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }

        //public virtual async Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate)
        //{
        //    return await _context.Set<T>().Where(predicate).ToListAsync();
        //}

        public virtual EntityEntry<T> Add(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            return _context.Set<T>().Add(entity);
        }


        //public virtual T Add(T entity)
        //{
        //    EntityEntry dbEntityEntry = _context.Entry<T>(entity);
        //    return _context.Set<T>().Add(entity);
        //}

        public virtual void Update(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Modified;
        }
        //public virtual void Delete(T entity)
        //{
        //    EntityEntry dbEntityEntry = _context.Entry<T>(entity);
        //    dbEntityEntry.State = EntityState.Deleted;
        //}
        //public void Delete(int id)
        //{
        //    var entity = _context.Set<T>().Find(id);
        //    dynamic e = entity;
        //    Type t = e.GetType();
        //    if (t.GetRuntimeProperty("Status") != null)
        //    {
        //        e.Status = false;
        //    }
        //    else
        //    {
        //        _context.Set<T>().Remove(entity);
        //    }
        //}

        public virtual EntityEntry<T> Delete(T entity)
        {
            return _context.Set<T>().Remove(entity);
        }
        public virtual EntityEntry<T> Delete(int id)
        {
            var entity = _context.Set<T>().Find(id);
            return _context.Set<T>().Remove(entity);
        }


        public virtual void Commit()
        {
            _context.SaveChanges();
        }

       


        public virtual IEnumerable<T> GetMulti(Expression<Func<T, bool>> predicate, string[] includes = null)
        {
            //HANDLE INCLUDES FOR ASSOCIATED OBJECTS IF APPLICABLE
            if (includes != null && includes.Count() > 0)
            {
                var query = _context.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include);
                return query.Where<T>(predicate).AsQueryable<T>();
            }

            return _context.Set<T>().Where<T>(predicate).AsQueryable<T>();
        }

        public bool CheckContains(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Count<T>(predicate) > 0;
        }
        public virtual int Count(Expression<Func<T, bool>> where)
        {
            return _context.Set<T>().Count(where);
        }

        public T GetSingleByCondition(Expression<Func<T, bool>> expression, string[] includes = null)
        {
            if (includes != null && includes.Count() > 0)
            {
                var query = _context.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include);
                return query.FirstOrDefault(expression);
            }
            return _context.Set<T>().FirstOrDefault(expression);
        }

        public virtual void DeleteMulti(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = _context.Set<T>().Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
                _context.Set<T>().Remove(obj);
        }

        //T IRepositoryBase<T>.Add(T entity)
        //{
        //    throw new NotImplementedException();
        //}

        public virtual T GetSingleById(int id)
        {
            return _context.Set<T>().Find(id);
        }
    }
}
