using BulkDataProcessingPlatform.Api.Enums;

namespace BulkDataProcessingPlatform.Api.Entities;

public class User : BaseEntity
{
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
    public Role Role { get; set; } = Role.User;
       
}