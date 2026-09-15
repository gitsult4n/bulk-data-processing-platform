using BulkDataProcessingPlatform.Api.Dtos.Auth;

namespace BulkDataProcessingPlatform.Api.Services;

public interface IAuthService
{
    Task<AuthResponse?> RegisterAsync(RegisterRequest request, CancellationToken ct);
    Task<AuthResponse?> LoginAsync(LoginRequest request, CancellationToken ct);
}
