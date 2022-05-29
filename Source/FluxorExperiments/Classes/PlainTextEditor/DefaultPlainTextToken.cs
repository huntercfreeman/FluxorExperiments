using FluxorExperiments.Classes.ImmutableStringBuilder;
using FluxorExperiments.Classes.KeyDownEvent;
using FluxorExperiments.Classes.Sequence;
using System.Text;

namespace FluxorExperiments.Classes.PlainTextEditor;

public record DefaultPlainTextToken(PlainTextTokenKey PlainTextTokenKey, SequenceKey SequenceKey) 
	: PlainTextTokenBase(PlainTextTokenKey, SequenceKey)
{
	// TODO: DefaultPlainTextToken needs to be immutable look into perhaps Span<T>?
	private readonly ImmutableStringBuilderRecord _immutableStringBuilderRecord;
	private readonly ImmutableStringBuilderRecordKey _immutableStringBuilderRecordKey;

	public DefaultPlainTextToken(KeyDownEventRecord keyDownEventRecord)
		: this(PlainTextTokenKey.NewPlainTextTokenKey(), SequenceKey.NewSequenceKey())
	{
		_immutableStringBuilderRecord = new();

		_immutableStringBuilderRecordKey = _immutableStringBuilderRecord
			.Append(keyDownEventRecord.Key);
	}	
	
	public DefaultPlainTextToken(KeyDownEventRecord keyDownEventRecord,
		DefaultPlainTextToken otherDefaultPlainTextToken)
		: this(otherDefaultPlainTextToken.PlainTextTokenKey, SequenceKey.NewSequenceKey())
	{
		_immutableStringBuilderRecord = otherDefaultPlainTextToken._immutableStringBuilderRecord;
		
		_immutableStringBuilderRecordKey = _immutableStringBuilderRecord
			.Append(keyDownEventRecord.Key);
	}	
	
	public override PlainTextTokenKind PlainTextTokenKind => PlainTextTokenKind.Default;
	public override string ToPlainText => _immutableStringBuilderRecord
		.GetString(_immutableStringBuilderRecordKey);
}