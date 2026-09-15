namespace BulkDataProcessingPlatform.Api.Dtos.Auth;

public record AuthResponse(string Token, DateTime ExpiresAt);
