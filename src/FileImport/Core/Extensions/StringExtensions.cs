using System.Text.RegularExpressions;
namespace FileImport.Core.Extensions;
public static class StringExtensions
{
	private static readonly Regex multipleSpacesRegex = CreateMultipleSpacesRegEx();

	public static string ReplaceMultipleSpacesWithSingle(this string str)
	{
		if (str == null)
			return str;

		return multipleSpacesRegex.Replace(str, " ");
	}

	private static Regex CreateMultipleSpacesRegEx()
	{
		var options = RegexOptions.Compiled;

		return new Regex("[ ]{2,}", options);
	}

}

