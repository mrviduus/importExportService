using FileImport.Core;
using FileImport.Core.Exceptions;
using FileImport.Core.Extensions;
using ImportExportCommon;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Reflection;
namespace FileImport;

public class FileImporter<T> : IFileImporter<T>
		where T : new()
{
	private const int MAX_ROWS = 100_000;
	private readonly IFileAdapterFactory fileAdapterFactory;
	private readonly ILogger<FileImporter<T>> logger;

	public FileImporter(IFileAdapterFactory fileAdapterFactory, ILogger<FileImporter<T>> logger)
	{
		this.fileAdapterFactory = fileAdapterFactory;
		this.logger = logger;
	}

	public FileValidationResult ValidateStructure(Stream stream, string fileExtension, FileStructure expectedFileStructure)
	{
		if (expectedFileStructure is null)
		{
			throw new ArgumentNullException(nameof(expectedFileStructure));
		}

		IFileAdapter fileParser = null;
		try
		{
			fileParser = fileAdapterFactory.GetFileAdapter(fileExtension);
			fileParser.Init(stream);

			if (fileParser.RowCount > MAX_ROWS)
			{
				return FileValidationResult.Error(FileErrorCodes.FILE_TOO_BIG, $"File is too big. Max count of rows is {MAX_ROWS}.");
			}

			// read row 1
			if (!fileParser.NextRow())
			{
				return FileValidationResult.Error(FileErrorCodes.HEADER_ROW_MISSING, "Failed to read row #1.");
			}

			// read row 2
			if (!fileParser.NextRow())
			{
				return FileValidationResult.Error(FileErrorCodes.HEADER_ROW_MISSING, "Failed to read row #2.");
			}

			var errors = new List<FileError>();
			var columnsFoundInFile = ReadHeadersFromFile(expectedFileStructure, fileParser, errors);

			var columnsMissingInFile = expectedFileStructure.Columns
				.Select(t => t.Value)
				.Except(columnsFoundInFile);

			if (columnsMissingInFile.Any())
			{
				var missingColumnsStr = string.Join(",", columnsMissingInFile.Select(t => t.NameInFile));
				errors.Add(new FileError(FileErrorCodes.HEADER_COLUMNS_MISSING, $"Columns [{missingColumnsStr}] are missing in file.", missingColumnsStr));
			}

			if (errors.Any())
				return new FileValidationResult(errors.ToArray());
			else
				return FileValidationResult.Ok();

		}
		catch (InvalidFileHeaderException)
		{
			return FileValidationResult.Error(FileErrorCodes.FILE_INVALID, "Failed to recognize an excel file.");
		}
		finally
		{
			fileParser?.Dispose();
		}
	}

	public IEnumerable<ParsedRow<T>> ReadFile(Stream stream, string fileExtension, FileStructure expectedFileStructure)
	{
		if (expectedFileStructure is null)
		{
			throw new ArgumentNullException(nameof(expectedFileStructure));
		}

		int rowIndex = 0;

		IFileAdapter fileParser = null;
		try
		{
			fileParser = fileAdapterFactory.GetFileAdapter(fileExtension);
			fileParser.Init(stream);

			// read info row
			fileParser.NextRow();
			rowIndex++;

			// read row with column headers
			fileParser.NextRow();
			rowIndex++;
			var columns = ReadHeadersFromFile(expectedFileStructure, fileParser, new List<FileError>());

			// read data rows
			while (fileParser.NextRow())
			{
				// TODO instatiate via expressions for better performance
				var obj = new T();
				var objContainer = new ParsedRow<T>(rowIndex + 1, obj);

				bool anyColumnSet = false;

				// we may have more or less cells in a certain row than we expect
				var countOfCellsToRead = Math.Min(fileParser.ColumnsCountInCurrentRow, columns.Length);
				for (int colIndex = 0; colIndex < countOfCellsToRead; colIndex++)
				{
					var columnConfig = columns[colIndex];
					if (columnConfig.IsDummy)
						continue;

					// GetValue() returns object
					var cellVal = fileParser.GetValue(colIndex);

					bool valueSet = SetPropertyValue(objContainer, cellVal, columnConfig);
					if (valueSet)
						anyColumnSet = true;
				}

				// stop reading if current row is empty
				if (!anyColumnSet)
					yield break;
				else
					yield return objContainer;

				rowIndex++;
			}
		}
		finally
		{
			fileParser?.Dispose();
		}
	}

	private FileColumn[] ReadHeadersFromFile(FileStructure fileStructure, IFileAdapter parser, IList<FileError> errors)
	{
		var headerColumns = new List<FileColumn>(fileStructure.Columns.Count);

		for (int colIndex = 0; colIndex < parser.ColumnsCountInCurrentRow; colIndex++)
		{
			string columnName = null;
			bool failedToParseColumn = false;
			try
			{
				columnName = parser.GetStringValue(colIndex);
			}
			catch (InvalidCastException ex)
			{
				failedToParseColumn = true;

				string errorMsg = $"Failed to parse header column at index [{colIndex}].";
				errors.Add(new FileError(FileErrorCodes.HEADER_VALUE_INVALID, errorMsg, colIndex.ToString(CultureInfo.InvariantCulture)));

				logger.LogError(ex, errorMsg);
			}

			// check next column even if exception occured
			if (failedToParseColumn)
				continue;

			// stop header reading as soon as we encounter empty cell
			if (!failedToParseColumn && columnName == null)
				break;

			if (columnName != null)
				columnName = columnName.Trim();

			if (fileStructure.Columns.TryGetValue(columnName, out FileColumn res))
			{
				headerColumns.Add(res);
			}
			else
			{
				headerColumns.Add(new FileColumn(true, columnName));
				logger.LogInformation($"Unknown column [{columnName}] found in file.");
			}
		}

		return headerColumns.ToArray();
	}

	private static bool SetPropertyValue(ParsedRow<T> objContainer, object cellVal, FileColumn columnConfig)
	{
		bool valueSet = false;

		// TODO use expressions for better performance or at least cache once first data row was parsed
		var propInfo = typeof(T).GetProperty(columnConfig.NameInModel, BindingFlags.Public | BindingFlags.Instance);
		var propType = propInfo.PropertyType;
		bool canPropBeNull = !propType.IsValueType || (Nullable.GetUnderlyingType(propType) != null);

		if (cellVal == null && !canPropBeNull)
		{
			objContainer.ErrorMessage = $"Cell {columnConfig.NameInFile} can not be empty.";
			objContainer.IsAnyError = true;
		}

		if (cellVal != null)
		{
			valueSet = true;
			if (propInfo.PropertyType != cellVal.GetType())
			{
				// TODO add checks
				cellVal = Convert.ChangeType(cellVal, propInfo.PropertyType, CultureInfo.InvariantCulture);
			}

			if (propInfo != null && propInfo.CanWrite)
			{
				if (cellVal is string stringCellVal && cellVal != null)
				{
					// remove leading, trailing spaces and replace more than 2+ spaces with a single one
					// we do it to avoid further errors when validating data, using strings as logins, etc...
					cellVal = stringCellVal.Trim().ReplaceMultipleSpacesWithSingle();
				}

				// TODO use expressions for better performance
				propInfo.SetValue(objContainer.Object, cellVal, null);
			}
		}

		return valueSet;
	}
}

