using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CSTS.Api.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IQueryable<T> GetAll<T>() where T : class;
        void Add<T>(T entity) where T : class;
        void AddRange<T>(IEnumerable<T> entities) where T : class;
        void Update<T>(T entity) where T : class;
        void UpdateRange<T>(IEnumerable<T> entities) where T : class;
        void Delete<T>(T entity) where T : class;
        void DeleteRange<T>(IEnumerable<T> entities) where T : class;
        Task<int> Commit();
    }
}
