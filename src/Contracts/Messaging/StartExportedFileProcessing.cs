namespace Contracts.Messaging;
public interface IStartExportedFileProcessing
{
	public int JobId { get; set; }

}

public class StartExportedFileProcessing : IStartImportedFileProcessing
{
	public int JobId { get; set; }
}

