using System.Collections;
using ImportExportCommon;

namespace WorkerService.Services.FileImport.Core;
public interface IFileStructureProvider
{
	string StructureCode { get; }
	FileStructure ProvideExportStructure();
	FileStructure ProvideImportStructure();
	IEnumerable ProvideSampleData();
}

