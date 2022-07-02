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
	public override ReadOnlySpan<char> AsPlainTextSpan => NextLine.AsSpan();
}