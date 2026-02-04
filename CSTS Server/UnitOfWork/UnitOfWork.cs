using CSTS.Api.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CSTS.Api.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CstsDbContext _context;

        public UnitOfWork(CstsDbContext context)
        {
            _context = context;
        }

        public IQueryable<T> GetAll<T>() where T : class
        {
            return _context.Set<T>();
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Set<T>().Add(entity);
        }

        public void AddRange<T>(IEnumerable<T> entities) where T : class
        {
            _context.Set<T>().AddRange(entities);
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Set<T>().Update(entity);
        }

        public void UpdateRange<T>(IEnumerable<T> entities) where T : class
        {
            _context.Set<T>().UpdateRange(entities);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Set<T>().Remove(entity);
        }

        public void DeleteRange<T>(IEnumerable<T> entities) where T : class
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public async Task<int> Commit()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
