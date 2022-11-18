namespace FileImport.Core;

public class FileError
{
	public string ErrorCode { get; }

	public string ErrorMessage { get; }

	public string ErrorData { get; }

	public FileError(string errorCode, string errorMessage, string errorData = null)
	{
		if (string.IsNullOrEmpty(errorCode))
		{
			throw new ArgumentException("Field can not be null or empty.", nameof(errorCode));
		}

		ErrorCode = errorCode;
		ErrorMessage = errorMessage;
		ErrorData = errorData;
	}
}

