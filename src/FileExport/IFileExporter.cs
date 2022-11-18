using ImportExportCommon;
using System.Collections;

namespace FileExport;
public interface IFileExporter
{
	/// <summary>
	/// Returns stream with excel file. Stream should be disposed by the calling code
	/// </summary>
	MemoryStream ExportData(FileStructure fileStructure, IEnumerable data);
}

public interface IFileExporter<in T>
{
	/// <summary>
	/// Returns stream with excel file. Stream should be disposed by the calling code
	/// </summary>
	MemoryStream ExportData(FileStructure fileStructure, IEnumerable<T> data);
}
