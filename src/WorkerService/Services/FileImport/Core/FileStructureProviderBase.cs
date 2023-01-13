using System.Collections;
using Contracts;
using DataAccess.Entity;
using ImportExportCommon;
using Newtonsoft.Json;

namespace WorkerService.Services.FileImport.Core;

public abstract class FileStructureProviderBase<T> : IFileStructureProvider
where T : BaseParseEntity
{
	public abstract string StructureCode { get; }

	public FileStructure ProvideImportStructure()
	{
		var builder = FileStructureBuilder.Build<T>(StructureCode);

		ConfigureColumns(builder);

		return builder.GetStructure();
	}

	public FileStructure ProvideExportStructure()
	{
		var builder = FileStructureBuilder.Build<T>(StructureCode);

		builder.AddColumn(
			"ColStatus",
			t => t.ErrorsJson,
			config => config.SetIsStatusColumn(true).SetExportTransformFunc(erJson => GetErrorMessageFromJson(erJson))
		);

		ConfigureColumns(builder);

		return builder.GetStructure();
	}

	public abstract IEnumerable ProvideSampleData();

	protected abstract void ConfigureColumns(FileStructureBuilder<T> builder);

	protected string GetErrorMessageFromJson(string errorsJson)
	{
		if (string.IsNullOrEmpty(errorsJson))
			return errorsJson;

		var errors = JsonConvert.DeserializeObject<List<ValidationError>>(errorsJson);

		return string.Join("; ", errors.Select(t => t.ErrorMessage));
	}
}
