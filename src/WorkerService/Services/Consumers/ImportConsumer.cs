using Contracts.Messaging;
using MassTransit;

namespace WorkerService.Services.Consumers;

public class ImportConsumer : IConsumer<IStartImportedFileProcessing>
{
	public Task Consume(ConsumeContext<IStartImportedFileProcessing> context)
	{
		throw new NotImplementedException();
	}
}

