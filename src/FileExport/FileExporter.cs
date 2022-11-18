using ClosedXML.Excel;
using FileExport.Exceptions;
using ImportExportCommon;
using System.Collections;
using System.Reflection;

namespace FileExport;

public class FileExporter<T> : FileExporter, IFileExporter<T>
{
	public MemoryStream ExportData(FileStructure fileStructure, IEnumerable<T> data)
	{
		return base.ExportData(fileStructure, data);
	}
}

public class FileExporter : IFileExporter
{
	public MemoryStream ExportData(FileStructure fileStructure, IEnumerable data)
	{
		if (fileStructure is null)
		{
			throw new ArgumentNullException(nameof(fileStructure));
		}

		if (data is null)
		{
			throw new ArgumentNullException(nameof(data));
		}

		var stream = new MemoryStream();

		using (var wb = new XLWorkbook())
		{
			var ws = wb.AddWorksheet("Data");

			int rowIndex = 1;
			int columnIndex = 1;

			// 1. write info row and apply column settings (like dropdown, etc.)
			foreach (var column in fileStructure.Columns)
			{
				var infoText = column.Value.ColumnInfo;
				if (infoText != null)
				{
					ws.Cell(rowIndex, columnIndex).Value = infoText;
				}
				ws.Cell(rowIndex, columnIndex).Style.Fill.BackgroundColor = XLColor.Blue;
				ws.Cell(rowIndex, columnIndex).Style.Font.FontColor = XLColor.White;
				ws.Cell(rowIndex, columnIndex).Style.Alignment.WrapText = true;

				// this column is a dropdown
				var dropDownConfig = column.Value.DropDown;
				if (dropDownConfig != null)
				{
					ConfigureDropDown(wb, ws, columnIndex, dropDownConfig);
				}

				columnIndex++;
			}

			// 2. write header row
			rowIndex++;
			columnIndex = 1;
			foreach (var column in fileStructure.Columns)
			{
				ws.Cell(rowIndex, columnIndex).Value = column.Value.NameInFile;
				ws.Cell(rowIndex, columnIndex).Style.Fill.BackgroundColor = XLColor.Yellow;
				ws.Cell(rowIndex, columnIndex).Style.Font.FontColor = XLColor.Red;

				columnIndex++;
			}

			// 3. write data rows
			IDictionary<string, PropertyInfo> properties = null;
			foreach (var currentObject in data)
			{
				rowIndex++;
				columnIndex = 1;

				if (properties == null)
				{
					properties = currentObject
						.GetType()
						.GetProperties(BindingFlags.Public | BindingFlags.Instance)
						.ToDictionary(t => t.Name, t => t);
				}

				foreach (var columnConfigDictEntry in fileStructure.Columns)
				{
					var columnConfig = columnConfigDictEntry.Value;
					var currentCell = ws.Cell(rowIndex, columnIndex);

					var propertyInfo = properties[columnConfig.NameInModel];
					var propertyType = columnConfig.TypeInModel;
					var propertyValue = propertyInfo.GetValue(currentObject);

					if (columnConfig.ExportTransformer != null)
					{
						propertyValue = columnConfig.ExportTransformer(propertyValue);
					}

					if (columnConfig.TypeInFile == FileDataType.Text)
					{
						if (propertyType == typeof(string))
						{
							// SetValue doesn't try to auto format value.
							// So string "123" won't be shown as number in excel file
							// see https://github.com/ClosedXML/ClosedXML/wiki/Text-with-numbers-are-getting-converted-to-numbers
							currentCell.SetValue(propertyValue as string);
						}
						else
							currentCell.Value = "'" + propertyValue;
					}
					else
						currentCell.Value = propertyValue;

					if (columnConfig.IsStatusColumn)
					{
						if (propertyType != typeof(string))
							throw new InvalidExportConfigurationException("Status column must be of type string.");

						StyleStatusColumn(currentCell, (string)propertyValue);
					}

					columnIndex++;
				}
			}

			// 4. adjust columns width so all text fits
			ws.Columns(1, fileStructure.Columns.Count + 1).AdjustToContents();

			wb.SaveAs(stream);
		}

		return stream;
	}

	private static void StyleStatusColumn(IXLCell cell, string propertyValue)
	{
		if (string.IsNullOrEmpty(propertyValue))
		{
			cell.Style.Fill.BackgroundColor = XLColor.FromTheme(XLThemeColor.Accent3);
		}
		else
		{
			cell.Style.Font.FontColor = XLColor.FromTheme(XLThemeColor.Background1);
			cell.Style.Fill.BackgroundColor = XLColor.FromTheme(XLThemeColor.Accent2);
		}
	}

	private static void ConfigureDropDown(IXLWorkbook workBook, IXLWorksheet mainWorkSheet, int currentColumnIndex, DropDownConfig dropDownConfig)
	{
		// more than 1 column may have the same dropdown
		var dropDownWorkSheet = workBook.Worksheets.FirstOrDefault(t => t.Name == dropDownConfig.SheetName);

		if (dropDownWorkSheet == null)
		{
			dropDownWorkSheet = workBook.AddWorksheet(dropDownConfig.SheetName);
			dropDownWorkSheet.Visibility = XLWorksheetVisibility.Hidden;

			// fill dropdown sheet with values
			for (int i = 0; i < dropDownConfig.values.Length; i++)
			{
				dropDownWorkSheet.Cell(i + 1, 1).Value = dropDownConfig.values[i];
			}
		}

		// set current column cell to be a dropdown
		var dropDownRange = dropDownWorkSheet.Range(
			dropDownWorkSheet.Cell(1, 1).Address,
			dropDownWorkSheet.Cell(dropDownConfig.values.Length, 1).Address
		);

		mainWorkSheet.Column(currentColumnIndex).SetDataValidation().List(dropDownRange, true);
	}
}
