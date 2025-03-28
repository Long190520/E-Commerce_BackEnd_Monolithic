using System.Linq.Expressions;

namespace E_Commerce_BackEnd.DAL
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T?> GetById(int id);
        T Create(T entity);
        T Update(T entity);
        void Delete(T entity);
        void DeleteWhere(Expression<Func<T, bool>> where);
    }
}
