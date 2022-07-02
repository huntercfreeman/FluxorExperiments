using System.Text;

namespace FluxorExperiments.ClassLibrary.Html;

public static class HtmlHelper
{
	public const string _spaceString = "&nbsp;";
	public const string _tabString = "&nbsp;&nbsp;&nbsp;&nbsp;";
	public const string _newLineString = "<br/>";
	public const string _ampersandString = "&amp;";
	public const string _leftAngleBracketString = "&lt;";
	public const string _rightAngleBracketString = "&gt;";
	public const string _doubleQuoteString = "&quot;";
	public const string _singleQuoteString = "&#39;";

	public static string EscapeHtml(this char input)
	{
		return new StringBuilder(input).EscapeHtml().ToString();
	}

	public static StringBuilder EscapeHtml(this StringBuilder input)
	{
		return input.Replace("&", _ampersandString)
			.Replace("<", _leftAngleBracketString)
			.Replace(">", _rightAngleBracketString)
			.Replace("\t", _tabString)
			.Replace(" ", _spaceString)
			.Replace("\n", _newLineString)
			.Replace("\"", _doubleQuoteString)
			.Replace("'", _singleQuoteString);
	}



	public static string ToHtmlEscapedString(this string input)
	{
		return new StringBuilder(input)
			.Replace("&", _ampersandString)
			.Replace("<", _leftAngleBracketString)
			.Replace(">", _rightAngleBracketString)
			.Replace("\t", _tabString)
			.Replace(" ", _spaceString)
			.Replace("\n", _newLineString)
			.Replace("\"", _doubleQuoteString)
			.Replace("'", _singleQuoteString).ToString();
	}

	//public static string ToHtmlEscapedString(this ReadOnlySpan<char> input)
	//{
	//	return new StringBuilder()
	//		.Append(input)
	//		.Replace("&", _ampersandString)
	//		.Replace("<", _leftAngleBracketString)
	//		.Replace(">", _rightAngleBracketString)
	//		.Replace("\t", _tabString)
	//		.Replace(" ", _spaceString)
	//		.Replace("\n", _newLineString)
	//		.Replace("\"", _doubleQuoteString)
	//		.Replace("'", _singleQuoteString).ToString();
	//}
}