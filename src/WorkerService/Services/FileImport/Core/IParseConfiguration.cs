namespace WorkerService.Services.FileImport.Core;

public delegate IParseConfiguration ParseConfigurationResolver(string name);

public interface IParseConfiguration
{
	IFileStructureProvider FileStructureProvider { get; }
}
