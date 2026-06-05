using BCrypt.Net;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SchoolManagement.Model.DTOs;
using SchoolManagement.Store.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SchoolManagement.Store.Repositories
{
    /// <summary>
    /// Handles Authentication and JWT Token Generation.
    /// </summary>
    public class AuthRepository : IAuthRepository
    {
        private readonly DatabaseContext _context;
        private readonly IConfiguration _configuration;

        public AuthRepository(
            DatabaseContext context,
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        /// <summary>
        /// Validates user credentials and generates JWT token.
        /// </summary>
        public async Task<LoginResponseDto?> LoginAsync(LoginDto loginDto)
        {
            try
            {
                using var connection = _context.CreateConnection();

                var query = @"
                    SELECT
                        U.UserId,
                        U.UserName,
                        U.PasswordHash,
                        R.RoleName
                    FROM mst_User U
                    INNER JOIN mst_Role R
                        ON U.RoleId = R.RoleId
                    WHERE U.UserName = @UserName
                    AND U.IsActive = 1";

                using var command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@UserName", loginDto.UserName);

                await connection.OpenAsync();

                using var reader = await command.ExecuteReaderAsync();

                if (!await reader.ReadAsync())
                    return null;

                var passwordHash =
                    reader["PasswordHash"]?.ToString() ?? string.Empty;

                // Verify BCrypt Password
                if (!BCrypt.Net.BCrypt.Verify(
                        loginDto.Password,
                        passwordHash))
                {
                    return null;
                }

                var userId = Convert.ToInt32(reader["UserId"]);
                var userName = reader["UserName"]?.ToString() ?? string.Empty;
                var roleName = reader["RoleName"]?.ToString() ?? string.Empty;

                var token = GenerateJwtToken(
                    userId,
                    userName,
                    roleName);

                return new LoginResponseDto
                {
                    Token = token
                };
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Error occurred during login.",
                    ex);
            }
        }

        /// <summary>
        /// Generates JWT Token.
        /// </summary>
        private string GenerateJwtToken(
            int userId,
            string userName,
            string roleName)
        {
            var jwtSettings =
                _configuration.GetSection("JwtSettings");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    jwtSettings["SecretKey"]!));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    userId.ToString()),

                new Claim(
                    ClaimTypes.Name,
                    userName),

                new Claim(
                    ClaimTypes.Role,
                    roleName)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(
                    Convert.ToDouble(
                        jwtSettings["ExpiryMinutes"])),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}