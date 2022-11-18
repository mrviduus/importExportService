using Contracts.Messaging;
using MassTransit;

namespace WorkerService.Services.Consumers;
public class ExportConsumer : IConsumer<IStartExportedFileProcessing>
{
	public Task Consume(ConsumeContext<IStartExportedFileProcessing> context)
	{
		throw new NotImplementedException();
	}
}

