using System.ComponentModel.DataAnnotations;

namespace BulkDataProcessingPlatform.Api.Dtos.Auth;

public record LoginRequest(
    [Required] string Username,
    [Required] string Password);
