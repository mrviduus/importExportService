using MassTransit;

namespace WorkerService.HostedServices;

internal class ImportExportHostedService : IHostedService
{
	private readonly IBusControl busControl;

	public ImportExportHostedService(IBusControl busControl) =>
		this.busControl = busControl;

	public async Task StartAsync(CancellationToken cancellationToken) =>
		await busControl.StartAsync(cancellationToken);

	public async Task StopAsync(CancellationToken cancellationToken) =>
		await busControl.StopAsync(cancellationToken);
}

