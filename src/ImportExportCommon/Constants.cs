namespace ImportExportCommon;
public static class Constants
{
	public const string UsersCode = "users";
}

public static class ContentType
{
	public const string xlsx = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

	public const string zip = "application/zip";
}

public static class FileExtensions
{
	public const string xlsx = "xlsx";
	
	public const string xls = "xls";
	
	public const string zip = "zip";

}

public static class FileErrorCodes
{
	public const string HEADER_VALUE_INVALID = "HEADER_VALUE_INVALID";
	public const string HEADER_COLUMNS_MISSING = "HEADER_COLUMNS_MISSING";
	public const string HEADER_ROW_MISSING = "HEADER_ROW_MISSING";
	public const string FILE_INVALID = "FILE_INVALID";
	public const string FILE_TOO_BIG = "FILE_TOO_BIG";
	public const string FILE_NO_DATA_ROWS = "FILE_NO_DATA_ROWS";
	public static readonly string DATA_VALIDATION_ERROR;
}