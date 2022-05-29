using FluxorExperiments.Classes.Sequence;

namespace FluxorExperiments.Classes.PlainTextEditor;

public record StartOfRowPlainTextToken(PlainTextTokenKey PlainTextTokenKey, SequenceKey SequenceKey) 
	: PlainTextTokenBase(PlainTextTokenKey, SequenceKey)
{
	public StartOfRowPlainTextToken()
		: this(PlainTextTokenKey.NewPlainTextTokenKey(), SequenceKey.NewSequenceKey())
	{
		
	}
	
	public override PlainTextTokenKind PlainTextTokenKind => PlainTextTokenKind.StartOfRow;
	public override string ToPlainText => "\n";
}