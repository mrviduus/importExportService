namespace Contracts;

public class ValidationError
{
	/// <summary>
	/// Validation required.
	/// </summary>
	public static readonly string VALIDATION_REQUIRED = nameof(VALIDATION_REQUIRED);

	/// <summary>
	/// Length is too big or too small, see error details for min and max length.
	/// </summary>
	public static readonly string VALIDATION_LENGTH_DB = nameof(VALIDATION_LENGTH_DB);

	/// <summary>
	/// Length is too big or too small, see error details for min and max length (for Enterprise).
	/// </summary>
	public static readonly string VALIDATION_LENGTH_BUSINESS = nameof(VALIDATION_LENGTH_BUSINESS);

	/// <summary>
	/// Something went wrong parsing this cell from file.
	/// </summary>
	public static readonly string VALIDATION_VALUE = nameof(VALIDATION_VALUE);

	/// <summary>
	/// This field should be unique in the whole file (see first occurence row number in error data).
	/// </summary>
	public static readonly string VALIDATION_UNIQUE = nameof(VALIDATION_UNIQUE);

	/// <summary>
	/// Invalid email.
	/// </summary>
	public static readonly string VALIDATION_EMAIL_FORMAT = nameof(VALIDATION_EMAIL_FORMAT);

	/// <summary>
	/// Invalid phone.
	/// </summary>
	public static readonly string VALIDATION_PHONE_FORMAT = nameof(VALIDATION_PHONE_FORMAT);

	public static readonly string ERROR_DATA_ROW_NUMBER_OCCURED_IN = "RowNumberOccuredIn";

	public string ErrorCode { get; }
	public string ErrorMessage { get; }


	public IDictionary<string, string> ErrorData { get; }

	public ValidationError(string errorCode, string errorMessage, IDictionary<string, string> errorData = null)
	{
		ErrorCode = errorCode ?? throw new ArgumentNullException(nameof(errorCode));
		ErrorMessage = errorMessage;
		ErrorData = errorData;
	}
}
