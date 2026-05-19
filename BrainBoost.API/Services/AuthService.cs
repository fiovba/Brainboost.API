using BrainBoost.API.Data;
using BrainBoost.API.DTOs.Auth;
using BrainBoost.API.Entities;
using BrainBoost.API.Helpers;
using BrainBoost.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BrainBoost.API.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly JwtHelper _jwtHelper;

    public AuthService(AppDbContext context, JwtHelper jwtHelper)
    {
        _context = context;
        _jwtHelper = jwtHelper;
    }

    public async Task<AuthResponseDto> GetMeAsync(int userId)
    {
        var user = await _context.Users
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null)
            throw new Exception("User tapılmadı.");

        var token = _jwtHelper.GenerateToken(user);

        return new AuthResponseDto
        {
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role.Name,
            Token = token
        };
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        var emailExists = await _context.Users
            .AnyAsync(x => x.Email == dto.Email);

        if (emailExists)
            throw new Exception("Bu email artıq istifadə olunur.");

        if (dto.Role == "Admin")
            throw new Exception("Admin rolu ilə qeydiyyat mümkün deyil.");

        if (dto.Role != "Student" && dto.Role != "Teacher")
            throw new Exception("Role yalnız Student və ya Teacher ola bilər.");

        var selectedRole = await _context.Roles
            .FirstOrDefaultAsync(x => x.Name == dto.Role);

        if (selectedRole == null)
            throw new Exception("Role tapılmadı.");

        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            RoleId = selectedRole.Id
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        user.Role = selectedRole;

        var token = _jwtHelper.GenerateToken(user);

        return new AuthResponseDto
        {
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role.Name,
            Token = token
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await _context.Users
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x => x.Email == dto.Email);

        if (user == null)
            throw new Exception("Email və ya şifrə yanlışdır.");

        var passwordValid = BCrypt.Net.BCrypt.Verify(
            dto.Password,
            user.PasswordHash
        );

        if (!passwordValid)
            throw new Exception("Email və ya şifrə yanlışdır.");

        var token = _jwtHelper.GenerateToken(user);

        return new AuthResponseDto
        {
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role.Name,
            Token = token
        };
    }
}