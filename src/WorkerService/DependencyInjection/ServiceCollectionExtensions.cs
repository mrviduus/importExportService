using MassTransit;
using WorkerService.Configuration.Options;
using WorkerService.HostedServices;
using WorkerService.Services.Consumers;

namespace WorkerService.DependencyInjection
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddBusProvider(this IServiceCollection services,
		IConfiguration configuration)
		{
			var rabbitMqOptions = configuration.GetSection("RabbitMq").Get<RabbitMqOptions>();

			services.AddMassTransit(x =>
			{
				x.AddConsumer<ImportConsumer>();

				if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
				{
					x.UsingInMemory((context, configurator) =>
					{
						configurator.ConfigureEndpoints(context);
					});
				}
				else
				{
					x.UsingRabbitMq((context, configurator) =>
					{
						configurator.ReceiveEndpoint(rabbitMqOptions.ApiClientGeneratorQueue, e =>
						{
							e.ConfigureConsumer<ImportConsumer>(context, c =>
							{
								c.UseConcurrentMessageLimit(rabbitMqOptions.ConcurrentMessageLimit);
							});
						}
						);
						configurator.ConfigureEndpoints(context);

						configurator.Host(new Uri(rabbitMqOptions.HostUrl), hostConfig =>
						{
							if (rabbitMqOptions.ConnectToNodes)
							{
								hostConfig.UseCluster(cluster =>
								{
									rabbitMqOptions.NodesAdresses.ForEach(x => cluster.Node(x));
								});
							}
							hostConfig.Username(rabbitMqOptions.Username);
							hostConfig.Password(rabbitMqOptions.Password);
						});

					});

				}
			});

			services.AddMassTransitHostedService(waitUntilStarted: true);

			services.AddSingleton<IHostedService, ImportExportHostedService>();

			return services;
		}
	}
}
