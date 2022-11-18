using System.Linq.Expressions;
namespace ImportExportCommon;

public static class FileStructureBuilder
{
	public static FileStructureBuilder<TModel> Build<TModel>(string structureName)
	{
		return new FileStructureBuilder<TModel>(structureName, new Dictionary<string, FileColumn>());
	}

}

public class FileStructureBuilder<TModel>
{
	private readonly string name;

	private readonly IDictionary<string, FileColumn> columns;

	private FileColumn currentColumn;

	internal FileStructureBuilder(string name, IDictionary<string, FileColumn> columns)
	{
		this.name = name;
		this.columns = columns;
	}

	public FileStructureBuilder<TModel> AddColumn<TProp>(string nameInFile, Expression<Func<TModel, TProp>> propertyExpression)
	{
		if (propertyExpression is null)
		{
			throw new ArgumentNullException(nameof(propertyExpression));
		}

		MemberExpression memberExp;
		if (propertyExpression.Body is UnaryExpression unaryExpression)
		{
			memberExp = unaryExpression.Operand as MemberExpression;
		}
		else
		{
			memberExp= propertyExpression.Body as MemberExpression;
		}

		currentColumn = new FileColumn(nameInFile, memberExp.Member.Name, memberExp.Type);
		columns.Add(nameInFile, currentColumn);

		return this;

	}

	public FileStructureBuilder<TModel> AddColumn<TProp>(
	string nameInFile,
	Expression<Func<TModel, TProp>> propertyExpression,
	Action<FileColumnBuilder<TModel, TProp>> columnBuildAction)
	{
		if (propertyExpression is null)
		{
			throw new ArgumentNullException(nameof(propertyExpression));
		}

		if (columnBuildAction is null)
		{
			throw new ArgumentNullException(nameof(columnBuildAction));
		}

		var columnBuilder = new FileColumnBuilder<TModel, TProp>(nameInFile, propertyExpression);

		columnBuildAction(columnBuilder);
		
		currentColumn = columnBuilder.GetColumn();
		columns.Add(nameInFile, currentColumn);

		return this;
	}

	public FileStructure GetStructure()
	{
		return new FileStructure(name, columns.Select(t => t.Value));
	}
}

