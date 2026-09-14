using BulkDataProcessingPlatform.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace BulkDataProcessingPlatform.Api.data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Process> Processes => Set<Process>();
}