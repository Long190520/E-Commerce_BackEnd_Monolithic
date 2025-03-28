using Microsoft.EntityFrameworkCore;
using E_Commerce_BackEnd.Models;

namespace E_Commerce_BackEnd.DAL
{
    public interface IUnitOfWork : IDisposable
    {
        MyDbContext _context { get; }
        IRepository<T> Repository<T>() where T : class;
        Task<bool> CommitAsync();
    }
    public class UnitOfWork : IUnitOfWork
    {
        public MyDbContext _context { get; }
        private readonly Dictionary<Type, object> _repositories;

        public UnitOfWork(MyDbContext context)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();
        }

        public IRepository<T> Repository<T>() where T : class
        {
            if (_repositories.ContainsKey(typeof(T)))
            {
                return _repositories[typeof(T)] as IRepository<T>;
            }

            var repository = new Repository<T>(_context);
            _repositories.Add(typeof(T), repository);
            return repository;
        }

        public async Task<bool> CommitAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
