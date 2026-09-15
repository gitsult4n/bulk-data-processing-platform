using System.ComponentModel.DataAnnotations;

namespace BulkDataProcessingPlatform.Api.Dtos.Auth;

public record RegisterRequest(
    [Required, MinLength(3), MaxLength(50)] string Username,
    [Required, MinLength(8), MaxLength(128)] string Password);
