using ImportExportCommon;
namespace FileImport.Core;

public interface IFileImporter<T>
	where T : new()
{
	FileValidationResult ValidateStructure(Stream fileStream, string fileExtension, FileStructure expectedFileStructure);
	IEnumerable<ParsedRow<T>> ReadFile(Stream fileStream, string fileExtension, FileStructure expectedFileStructure);
}

