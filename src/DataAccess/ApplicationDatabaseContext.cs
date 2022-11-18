using System.Reflection;
using DataAccess.Entity;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class ApplicationDatabaseContext: DbContext
{
    public DbSet<Job> Jobs { get; set; }

    public DbSet<StoredFile> Files { get; set; }
    
    public DbSet<ParsedUser> ParsedUser { get; set; }
    
    public ApplicationDatabaseContext(DbContextOptions<ApplicationDatabaseContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}