using CSTS.Api.Data;
using CSTS.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;

namespace CSTS.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly CstsDbContext _context;

        public UserRepository(CstsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAdminUsersAsync()
        {
            return await _context.Users.Where(u => u.Role == UserRole.Admin).ToListAsync();
        }

        public async Task<User?> GetUserByUsernameAndPasswordAsync(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return user;
            }
            return null;
        }
    }
}

