using BulkDataProcessingPlatform.Api.Enums;

namespace BulkDataProcessingPlatform.Api.Entities;


public class Process : BaseEntity
{
    public Status Status { get; set; } = Status.OnProcess;
    public int SuccessCount { get; set; } = 0;
    public int FailureCount { get; set; } = 0;
    public string InvalidReason { get; set; } = string.Empty;
    
    
    
    

}