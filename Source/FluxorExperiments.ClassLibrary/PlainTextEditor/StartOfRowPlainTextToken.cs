using FluxorExperiments.ClassLibrary.Html;

namespace FluxorExperiments.ClassLibrary.PlainTextEditor;

public record StartOfRowPlainTextToken(int? IndexInPlainText) 
	: PlainTextTokenBase(IndexInPlainText)
{
	public const string NextLine = "\n";

	public StartOfRowPlainTextToken()
		: this(0)
	{
		
	}
	
	public override PlainTextTokenKind PlainTextTokenKind => PlainTextTokenKind.StartOfRow;
	public override ReadOnlySpan<char> PlainTextSpan => NextLine.AsSpan();

	public override int PlanTextLength => 1;

	public override string PlainText => NextLine;

	public override string ToHtmlEscapdString()
	{
		return HtmlHelper._newLineString;
	}
}