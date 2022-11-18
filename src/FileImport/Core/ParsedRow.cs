namespace FileImport.Core;

public class ParsedRow<T>
{
	public int RowNumber { get; }

	public bool IsAnyError { get; set; }

	public string ErrorMessage { get; set; }

	public T Object { get; }

	public ParsedRow(int rowNumber, T row)
	{
		RowNumber = rowNumber;
		Object = row;
	}
}

