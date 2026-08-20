using CRN.Product.API.Auth;
using CRN.Product.API.Data;
using CRN.Product.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace CRN.Product.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtService _jwtService;
        private readonly IConfiguration _configuration;

        public AuthController(
            ApplicationDbContext context,
            IJwtService jwtService,
            IConfiguration configuration)
        {
            _context = context;
            _jwtService = jwtService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u =>
                    u.Username == request.Username);

            if (user == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid username or password."
                });
            }

            var passwordValid = VerifyPassword(
                request.Password,
                user.PasswordHash);

            if (!passwordValid)
            {
                return Unauthorized(new
                {
                    message = "Invalid username or password."
                });
            }

            var accessToken =
                _jwtService.GenerateAccessToken(user);

            var refreshToken =
                _jwtService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;

            var refreshTokenDays =
                int.Parse(
                    _configuration["Jwt:RefreshTokenDays"] ?? "7");

            user.RefreshTokenExpiryTime =
                DateTime.UtcNow.AddDays(refreshTokenDays);

            await _context.SaveChangesAsync();

            var accessTokenMinutes =
                int.Parse(
                    _configuration["Jwt:AccessTokenMinutes"] ?? "15");

            return Ok(new LoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiry =
                    DateTime.UtcNow.AddMinutes(accessTokenMinutes)
            });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(
         RefreshTokenRequestDto request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u =>
                    u.RefreshToken == request.RefreshToken);

            if (user == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid refresh token."
                });
            }

            if (user.RefreshTokenExpiryTime == null ||
                user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return Unauthorized(new
                {
                    message = "Refresh token has expired."
                });
            }

            var accessToken =
                _jwtService.GenerateAccessToken(user);

            // Rotate refresh token
            var newRefreshToken =
                _jwtService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;

            var refreshTokenDays =
                int.Parse(
                    _configuration["Jwt:RefreshTokenDays"] ?? "7");

            user.RefreshTokenExpiryTime =
                DateTime.UtcNow.AddDays(refreshTokenDays);

            await _context.SaveChangesAsync();

            var accessTokenMinutes =
                int.Parse(
                    _configuration["Jwt:AccessTokenMinutes"] ?? "15");

            return Ok(new LoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken,
                AccessTokenExpiry =
                    DateTime.UtcNow.AddMinutes(accessTokenMinutes)
            });
        }
        private static bool VerifyPassword(
          string password,
          string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(
                password,
                passwordHash);
        }
    }
}