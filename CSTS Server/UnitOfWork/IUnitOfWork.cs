using CSTS.Api.Repositories;
using System;
using System.Threading.Tasks;

namespace CSTS.Api.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        ITicketRepository Tickets { get; }
        IUserRepository Users { get; }
        IRepository<T> Repository<T>() where T : class;
        Task<int> CompleteAsync();
    }
}

