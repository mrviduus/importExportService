using FileImport.Core;
using FileImport.Core.ConcreteFileAdapters;

namespace FileImport;
public class FileAdapterFactory : IFileAdapterFactory
{
	public IFileAdapter GetFileAdapter(string fileExtension)
	{
		if (string.IsNullOrEmpty(fileExtension))
		{
			throw new ArgumentException($"'{nameof(fileExtension)}' cannot be null or empty.", nameof(fileExtension));
		}

		switch (fileExtension)
		{
			case "xls":
			case "xlsx": return new ExcelDataReaderFileAdapter();
			default:
				throw new NotImplementedException($"{fileExtension} is not supported.");
		}
	}
}
