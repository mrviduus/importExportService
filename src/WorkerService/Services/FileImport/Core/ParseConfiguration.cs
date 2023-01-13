namespace WorkerService.Services.FileImport.Core;

public class ParseConfiguration : IParseConfiguration
{
	public IFileStructureProvider FileStructureProvider { get; }

	public ParseConfiguration(IFileStructureProvider fileStructureProvider)
	{
		FileStructureProvider = fileStructureProvider;
	}
}
