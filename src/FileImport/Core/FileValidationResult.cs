namespace FileImport.Core;

public class FileValidationResult
{
	public bool IsSuccessful { get; }

	public FileError [] Errors { get; }

	public FileValidationResult(bool isSuccessful)
	{
		IsSuccessful = isSuccessful;
	}

	public FileValidationResult(FileError[] errors) : this(false)
	{
		if (errors is null || !errors.Any())
		{
			throw new ArgumentException("Errors can not be null or empty", nameof(errors));
		}

		Errors = errors;
	}

	public static FileValidationResult Error(string errorCode, string errorMessage, string errorData = null)
	{
		if (errorCode is null)
		{
			throw new System.ArgumentNullException(nameof(errorCode));
		}

		return new FileValidationResult(
			new FileError[]
			{
				new FileError(errorCode, errorMessage, errorData)
			});
	}

	public static FileValidationResult Ok()
	{
		return new FileValidationResult(true);
	}

}

