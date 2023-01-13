using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection ConfigureDb(this IServiceCollection services,
		IConfiguration configuration,
		string dbConnectionStringName = "DBConnectionString")
	{
		services.AddDbContext<ApplicationDatabaseContext>(options =>
		{
			options.UseSqlite(configuration.GetConnectionString(dbConnectionStringName));
		});

		services.AddHealthChecks()
			.AddSqlite(configuration.GetConnectionString(dbConnectionStringName));

		return services;
	}
}