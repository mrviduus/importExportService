namespace FileImport.Core;

/// <summary>
/// Adapter between parse library and our code
/// </summary>
public interface IFileAdapter : IDisposable
{
	int RowCount { get; }

	int ColumnsCountInCurrentRow { get; }

	void Init(Stream stream);

	object GetValue(int columnIndex);

	string GetStringValue(int columnIndex);

	bool NextRow();	
}

