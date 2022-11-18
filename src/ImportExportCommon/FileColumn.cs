using System.Diagnostics;

namespace ImportExportCommon;

[DebuggerDisplay("{NameInModel}={NameInFile}, IsDummy={IsDummy}")]
public class FileColumn
{	
	public string NameInFile { get; }

	public string NameInModel { get; }

	public Type TypeInModel { get; }

	public FileDataType TypeInFile { get; private set; } = FileDataType.Text;

	//export only props
	public string ColumnInfo { get; private set; }

	public Func<object, object> ExportTransformer { get; private set; }
	
	public DropDownConfig DropDown { get; private set; }

	/// <summary>
	/// Column contains error text. If value is null/empty column will be green, otherwise - red
	/// </summary>
	public bool IsStatusColumn { get; private set; }

	/// <summary>
	/// Column Exist in file is skipped durring processing
	/// </summary>
	public bool IsDummy { get; }

	public FileColumn(string nameInFile, string nameInModel, Type typeInModel)
	{
		NameInFile = nameInFile;
		NameInModel = nameInModel;
		TypeInModel = typeInModel;
	}

	public FileColumn(bool isDummy, string nameInFile)
	{
		IsDummy = isDummy;
		NameInFile = nameInFile;
	}

	internal void SetFileDataType(FileDataType typeInFile)
	{
		TypeInFile = typeInFile;
	}

	internal void SetColumnInfo(string columnInfo)
	{
		ColumnInfo = columnInfo ?? throw new ArgumentNullException(nameof(columnInfo));
	}

	internal void SetStatusColumn(bool isStatusColumn = true)
	{
		IsStatusColumn	= isStatusColumn;
	}

	internal void SetExportTransformer(Func<object, object> exportTransformer)
	{
		ExportTransformer = exportTransformer ?? throw new ArgumentNullException(nameof(exportTransformer));
	}

	internal void SetDropDown(DropDownConfig dropDown)
	{
		DropDown = dropDown ?? throw new ArgumentNullException(nameof(dropDown));
	}



}
