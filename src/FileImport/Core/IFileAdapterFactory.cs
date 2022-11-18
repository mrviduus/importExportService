namespace FileImport.Core;

public interface IFileAdapterFactory
{
	IFileAdapter GetFileAdapter(string fileExtension);
}

