using Microsoft.EntityFrameworkCore;

namespace DataAccess;

internal class ApplicationDatabaseContextFactory
{
    public static ApplicationDatabaseContext CreateContext(string connectionString)
    {
        DbContextOptions<ApplicationDatabaseContext> options = GetDbContextOptions(connectionString);

        return new ApplicationDatabaseContext(options);
    }

    private static DbContextOptions<ApplicationDatabaseContext> GetDbContextOptions(string connectionString)
    {
        var dbContextOptionsBuilder = new DbContextOptionsBuilder<ApplicationDatabaseContext>();
        dbContextOptionsBuilder.UseSqlite(connectionString);

        return dbContextOptionsBuilder.Options;
    }
}