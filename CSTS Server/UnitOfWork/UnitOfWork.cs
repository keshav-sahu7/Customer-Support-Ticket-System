using CSTS.Api.Data;
using CSTS.Api.Repositories;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace CSTS.Api.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CstsDbContext _context;
        private Hashtable _repositories;
        public ITicketRepository Tickets { get; }
        public IUserRepository Users { get; }

        public UnitOfWork(CstsDbContext context)
        {
            _context = context;
            _repositories = new Hashtable();
            Tickets = new TicketRepository(_context);
            Users = new UserRepository(_context);
        }

        public IRepository<T> Repository<T>() where T : class
        {
            if (_repositories == null)
            {
                _repositories = new Hashtable();
            }

            var type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context);
                if (repositoryInstance != null)
                {
                    _repositories.Add(type, repositoryInstance);
                }
            }

            return (IRepository<T>)_repositories[type]!;
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

