using CSTS.Api.Data.Entities;
using CSTS.Api.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;

namespace CSTS.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<User>> GetAdminUsersAsync()
        {
            return await _unitOfWork.GetAll<User>().Where(u => u.Role == UserRole.Admin).ToListAsync();
        }

        public async Task<User?> GetUserByUsernameAndPasswordAsync(string username, string password)
        {
            var user = await _unitOfWork.GetAll<User>().FirstOrDefaultAsync(u => u.Username == username);
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash, false, BCrypt.Net.HashType.SHA256))
            {
                return user;
            }
            return null;
        }
    }
}

