using ExcelDataReader;
using ExcelDataReader.Exceptions;
using FileImport.Core.Exceptions;

namespace FileImport.Core.ConcreteFileAdapters;

internal class ExcelDataReaderFileAdapter : IFileAdapter
{
	private IExcelDataReader reader;
	public int RowCount 
	{
		get 
		{
			if (reader is null)
			{
				throw new InvalidOperationException($"{nameof(reader)} was not instatiated.");
			}

			return reader.RowCount; 
		}
	}

	public int ColumnsCountInCurrentRow 
	{
		get
		{
			if (reader is null)
			{
				throw new InvalidOperationException($"{nameof(reader)} was not instatiated.");
			}

			return reader.FieldCount;
		}
	}

	public void Dispose()
	{
		reader?.Dispose();
	}

	public string GetStringValue(int columnIndex)
	{
		return reader.GetString(columnIndex);
	}

	public object GetValue(int columnIndex)
	{
		var cellVal = reader.GetValue(columnIndex);

		return cellVal;
	}

	public void Init(Stream stream)
	{
		var readerConfig = new ExcelReaderConfiguration()
		{
			// do not close underlying stream
			LeaveOpen = true
		};

		try
		{
			reader = ExcelReaderFactory.CreateReader(stream, readerConfig);
		}
		catch (HeaderException ex)
		{

			throw new InvalidFileHeaderException("Failed to parse file headers.", ex);
		}
	}

	public bool NextRow()
	{
		return reader.Read();
	}

}

