using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BulkDataProcessingPlatform.Api.Data;
using BulkDataProcessingPlatform.Api.Dtos.Auth;
using BulkDataProcessingPlatform.Api.Entities;
using BulkDataProcessingPlatform.Api.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BulkDataProcessingPlatform.Api.Services;

public class AuthService(
    AppDbContext db,
    IPasswordHasher<User> hasher,
    IConfiguration configuration) : IAuthService
{
    private readonly IConfigurationSection _jwt = configuration.GetSection("Jwt");

    public async Task<AuthResponse?> RegisterAsync(RegisterRequest request, CancellationToken ct)
    {
        var username = request.Username.Trim();

        if (await db.Users.AnyAsync(u => u.Username == username, ct))
            return null;

        var user = new User
        {
            Username = username,
            PasswordHash = string.Empty,
            Role = Role.User
        };
        user.PasswordHash = hasher.HashPassword(user, request.Password);
        user.CreatedBy = user.Id;

        db.Users.Add(user);

        try
        {
            await db.SaveChangesAsync(ct);
        }
        catch (DbUpdateException)
        {
            return null;
        }

        return CreateToken(user);
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request, CancellationToken ct)
    {
        var username = request.Username.Trim();

        var user = await db.Users.FirstOrDefaultAsync(u => u.Username == username, ct);
        if (user is null)
            return null;

        var result = hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (result == PasswordVerificationResult.Failed)
            return null;

        if (result == PasswordVerificationResult.SuccessRehashNeeded)
        {
            user.PasswordHash = hasher.HashPassword(user, request.Password);
            user.UpdatedAt = DateTime.UtcNow;
            user.UpdatedBy = user.Id;
            await db.SaveChangesAsync(ct);
        }

        return CreateToken(user);
    }

    private AuthResponse CreateToken(User user)
    {
        var key = _jwt["Key"] ?? throw new InvalidOperationException("Jwt:Key is not configured.");
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Role, user.Role.ToString())
        };

        var expiresAt = DateTime.UtcNow.AddMinutes(int.Parse(_jwt["ExpiryMinutes"] ?? "60"));

        var token = new JwtSecurityToken(
            issuer: _jwt["Issuer"],
            audience: _jwt["Audience"],
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expiresAt,
            signingCredentials: credentials);

        return new AuthResponse(new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
    }
}
