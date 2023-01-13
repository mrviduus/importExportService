using Microsoft.EntityFrameworkCore.Design;

namespace DataAccess.Design;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDatabaseContext>
{
	public ApplicationDatabaseContext CreateDbContext(string[] args)
	{
		var connectionString = (args != null && args.Length > 0 && !string.IsNullOrEmpty(args[0]))
			? args[0]
			: "ConnectionString";

		return ApplicationDatabaseContextFactory.CreateContext(connectionString);
		//dotnet ef database update --connection "Data Source=C:/SQLiteStudio/importexport.sqlite;"
	}
}