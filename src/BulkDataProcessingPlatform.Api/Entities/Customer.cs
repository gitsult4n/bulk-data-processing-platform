namespace BulkDataProcessingPlatform.Api.Entities;

public class Customer : BaseEntity
{
    public  string Name { get; set; }
    public  string Email { get; set; }
    public  string PhoneNumber { get; set; }
    public  string City { get; set; }
}