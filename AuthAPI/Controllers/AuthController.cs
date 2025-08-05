using AuthAPI.Models;
using AuthAPI.Data;
using AuthAPI.Models.Entities;
using AuthAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthDbContext _context;
        private readonly JwtService _jwt;

        public AuthController(AuthDbContext context, JwtService jwt)
        {
            _context = context;
            _jwt = jwt;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            var exists = await _context.Users.AnyAsync(u => u.Username == dto.CUSERNAME);
            if (exists)
                return BadRequest(new { message = "Username already exists" });

            var user = new AuthEntity
            {
                Username = dto.CUSERNAME,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.CPASSWORD),
                Role = dto.CROLE
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Registration successful" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.CUSERNAME);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.CPASSWORD, user.PasswordHash))
                return Unauthorized(new { message = "Invalid username or password" });

            var token = _jwt.GenerateToken(user);
            return Ok(new { token });
        }
    }
}