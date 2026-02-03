using CSTS.Api.Data.Entities;
using CSTS.Api.Dtos; // Add this using directive
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSTS.Api.Services
{
    public interface IUserService
    {
        Task<LoginResponseDto?> LoginAsync(string username, string password); // Changed return type
        Task<IEnumerable<User>> GetAdminUsersAsync();
    }
}
