namespace ImportExportCommon;

public class FileStructure
{
	public string Name { get; }

	public IReadOnlyDictionary<string, FileColumn> Columns { get; }

	public FileStructure(string name, IEnumerable<FileColumn> columns)
	{
		Name = name;
		Columns = columns.ToDictionary(t => t.NameInFile, t => t, StringComparer.OrdinalIgnoreCase);
	}

}

