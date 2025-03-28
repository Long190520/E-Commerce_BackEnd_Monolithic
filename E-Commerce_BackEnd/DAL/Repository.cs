using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using E_Commerce_BackEnd.Models;
using System.Threading.Tasks;

namespace E_Commerce_BackEnd.DAL
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected DbContext _db;
        private readonly DbSet<T> dbset;

        public Repository(DbContext dataContext)
        {
            _db = dataContext;
            dbset = _db.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await dbset.ToListAsync();
        }

        public virtual async Task<T?> GetById(int id)
        {
            return await dbset.FindAsync(id);
        }

        public virtual T Create(T entity)
        {
            dbset.Add(entity);
            return entity;
        }

        public virtual T Update(T entity)
        {
            dbset.Update(entity);
            return entity;
        }

        public virtual void Delete(T entity)
        {
            dbset.Remove(entity);
        }

        public virtual void DeleteWhere(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> entityList = dbset.Where(where).AsEnumerable();
            foreach (var entity in entityList)
            {
                dbset.Remove(entity);
            }
            _db.SaveChanges();
        }
    }
}
