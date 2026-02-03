using CSTS.Api.Data.Entities;
using CSTS.Api.Dtos;
using CSTS.Api.UnitOfWork;
using Microsoft.Extensions.Configuration; // Added for IConfiguration
using Microsoft.IdentityModel.Tokens; // Added for SymmetricSecurityKey
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt; // Added for JwtSecurityToken
using System.Security.Claims; // Added for Claims
using System.Text; // Added for Encoding
using System.Threading.Tasks;

namespace CSTS.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration; // Injected IConfiguration

        public UserService(IUnitOfWork unitOfWork, IConfiguration configuration) // Constructor updated
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<IEnumerable<User>> GetAdminUsersAsync()
        {
            return await _unitOfWork.Users.GetAdminUsersAsync();
        }

        public async Task<LoginResponseDto?> LoginAsync(string username, string password) // Return type changed
        {
            var user = await _unitOfWork.Users.GetUserByUsernameAndPasswordAsync(username, password);

            if (user == null)
            {
                return null; // Authentication failed
            }

            // Generate JWT token
            var token = GenerateJwtToken(user);

            return new LoginResponseDto
            {
                UserId = user.Id.ToString(),
                Username = user.Username,
                Role = user.Role.ToString(),
                Token = token
            };
        }

        private string GenerateJwtToken(User user)
        {
            var jwtKey = _configuration["Jwt:Key"];
            var jwtIssuer = _configuration["Jwt:Issuer"];
            var jwtAudience = _configuration["Jwt:Audience"];

            if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
            {
                throw new InvalidOperationException("JWT configuration is missing.");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.Now.AddHours(1), // Token valid for 1 hour
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
