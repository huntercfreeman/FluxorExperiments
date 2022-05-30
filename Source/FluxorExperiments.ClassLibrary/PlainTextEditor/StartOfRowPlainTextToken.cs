using FluxorExperiments.ClassLibrary.Sequence;

namespace FluxorExperiments.ClassLibrary.PlainTextEditor;

public record StartOfRowPlainTextToken(PlainTextTokenKey PlainTextTokenKey, int? IndexInPlainText, SequenceKey SequenceKey) 
	: PlainTextTokenBase(PlainTextTokenKey, IndexInPlainText, SequenceKey)
{
	public StartOfRowPlainTextToken()
		: this(PlainTextTokenKey.NewPlainTextTokenKey(), 0, SequenceKey.NewSequenceKey())
	{
		
	}
	
	public override PlainTextTokenKind PlainTextTokenKind => PlainTextTokenKind.StartOfRow;
	public override string ToPlainText => "\n";
}