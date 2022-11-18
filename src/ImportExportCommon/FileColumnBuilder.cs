using System.Linq.Expressions;
namespace ImportExportCommon;

public class FileColumnBuilder<TModel, TProp>
{
	private readonly FileColumn currentColumn;

	public FileColumnBuilder(string nameInFile, Expression<Func<TModel, TProp>> propertyExpression)
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
			memberExp = propertyExpression.Body as MemberExpression;
		}

		currentColumn = new FileColumn(nameInFile, memberExp.Member.Name, memberExp.Type);

	}

	public FileColumnBuilder<TModel, TProp> SetFileDataType(FileDataType typeInFile)
	{
		currentColumn.SetFileDataType(typeInFile);

		return this;
	}

	public FileColumnBuilder<TModel, TProp> SetColumnInfo(string columnInfo)
	{
		currentColumn.SetColumnInfo(columnInfo);

		return this;
	}

	public FileColumnBuilder<TModel, TProp> SetIsStatusColumn(bool isStatus)
	{
		currentColumn.SetStatusColumn(isStatus);

		return this;
	}

	public FileColumnBuilder<TModel, TProp> SetExportTransformFunc(Func<TProp, object> exportTransformFunc)
	{
		currentColumn.SetExportTransformer(val => exportTransformFunc((TProp)val));

		return this;
	}

	internal FileColumn GetColumn()
	{
		return currentColumn;
	}
}