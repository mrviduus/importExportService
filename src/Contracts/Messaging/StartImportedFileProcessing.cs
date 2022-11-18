namespace Contracts.Messaging;
public interface IStartImportedFileProcessing
{
	public int JobId { get; set; }

}

public class StartImportedFileProcessing : IStartImportedFileProcessing
{
	public int JobId { get; set; }
}

