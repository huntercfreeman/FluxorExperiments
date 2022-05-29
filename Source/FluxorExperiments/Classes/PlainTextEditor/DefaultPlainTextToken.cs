using FluxorExperiments.Classes.KeyDownEvent;
using FluxorExperiments.Classes.Sequence;
using System.Text;

namespace FluxorExperiments.Classes.PlainTextEditor;

public record DefaultPlainTextToken(PlainTextTokenKey PlainTextTokenKey, SequenceKey SequenceKey) 
	: PlainTextTokenBase(PlainTextTokenKey, SequenceKey)
{
	// TODO: DefaultPlainTextToken needs to be immutable look into perhaps Span<T>?
	private readonly StringBuilder _contentBuilder;

	public DefaultPlainTextToken(KeyDownEventRecord keyDownEventRecord)
		: this(PlainTextTokenKey.NewPlainTextTokenKey(), SequenceKey.NewSequenceKey())
	{
		_contentBuilder = new(keyDownEventRecord.Key);
	}	
	
	public DefaultPlainTextToken(KeyDownEventRecord keyDownEventRecord,
		DefaultPlainTextToken otherDefaultPlainTextToken)
		: this(otherDefaultPlainTextToken.PlainTextTokenKey, SequenceKey.NewSequenceKey())
	{
		_contentBuilder = new(otherDefaultPlainTextToken.ToPlainText + keyDownEventRecord.Key);
	}	
	
	public override PlainTextTokenKind PlainTextTokenKind => PlainTextTokenKind.Default;
	public override string ToPlainText => _contentBuilder.ToString();
}