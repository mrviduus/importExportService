using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
	.MinimumLevel.Information()
	.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
	.MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
	.Enrich.FromLogContext()
	.WriteTo.File("./logs/log-.txt", rollingInterval: RollingInterval.Day)
	.WriteTo.Console()
	.CreateLogger();

try
{
	Log.Information("Starting the Service");

	IHost host = Host.CreateDefaultBuilder(args)
		.ConfigureAppConfiguration((hostContext, configBuilder) =>
		{
			configBuilder
				//.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json")
				.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
				.AddJsonFile($"appsettings.{Environment.MachineName}.json", true, true)
				.Build();
		})
		.UseSerilog((context, services, configuration) => configuration
			.ReadFrom.Configuration(context.Configuration)
			.ReadFrom.Services(services)
			.Enrich.FromLogContext())
		.ConfigureServices((hostContext, services) =>
		{
			//... 
		})
		.Build();


	await host.RunAsync();
}
catch (Exception ex)
{
	Log.Fatal(ex, "There was a problem starting the service");
}
finally
{
	Log.Information("Service successfully stopped");

	Log.CloseAndFlush();
}
