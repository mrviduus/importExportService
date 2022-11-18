using System.Runtime.Serialization;

namespace FileImport.Core.Exceptions;

public class InvalidFileHeaderException : Exception
{
	public InvalidFileHeaderException()
	{
	}

	public InvalidFileHeaderException(string message) : base(message)
	{
	}

	public InvalidFileHeaderException(string message, Exception innerException) : base(message, innerException)
	{
	}

	protected InvalidFileHeaderException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}


}

