namespace FileExport.Exceptions;
public class InvalidExportConfigurationException : Exception
{
	public InvalidExportConfigurationException()
	{
	}

	public InvalidExportConfigurationException(string message) : base(message)
	{
	}

	public InvalidExportConfigurationException(string message, Exception innerException) : base(message, innerException)
	{
	}
}

