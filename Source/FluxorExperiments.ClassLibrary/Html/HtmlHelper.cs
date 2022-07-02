using System.Text;

namespace FluxorExperiments.ClassLibrary.Html;

public static class HtmlHelper
{
	private const string _spaceString = "&nbsp;";
	private const string _tabString = "&nbsp;&nbsp;&nbsp;&nbsp;";
	private const string _newLineString = "<br/>";
	private const string _ampersandString = "&amp;";
	private const string _leftAngleBracketString = "&lt;";
	private const string _rightAngleBracketString = "&gt;";
	private const string _doubleQuoteString = "&quot;";
	private const string _singleQuoteString = "&#39;";

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

	public static string ToHtmlEscapedString(this ReadOnlySpan<char> input)
	{
		return new StringBuilder(input.ToString())
			.Replace("&", _ampersandString)
			.Replace("<", _leftAngleBracketString)
			.Replace(">", _rightAngleBracketString)
			.Replace("\t", _tabString)
			.Replace(" ", _spaceString)
			.Replace("\n", _newLineString)
			.Replace("\"", _doubleQuoteString)
			.Replace("'", _singleQuoteString).ToString();
	}
}