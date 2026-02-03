using CSTS.Api.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSTS.Api.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByUsernameAndPasswordAsync(string username, string password);
        Task<IEnumerable<User>> GetAdminUsersAsync();
    }
}

