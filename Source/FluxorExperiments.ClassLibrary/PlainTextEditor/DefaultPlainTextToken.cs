using FluxorExperiments.ClassLibrary.ImmutableStringBuilder;
using FluxorExperiments.ClassLibrary.KeyDownEvent;
using FluxorExperiments.ClassLibrary.Sequence;

namespace FluxorExperiments.ClassLibrary.PlainTextEditor;

public record DefaultPlainTextToken(PlainTextTokenKey PlainTextTokenKey, int? IndexInContent, SequenceKey SequenceKey) 
	: PlainTextTokenBase(PlainTextTokenKey, IndexInContent, SequenceKey)
{
	// TODO: DefaultPlainTextToken needs to be immutable look into perhaps Span<T>?
	private readonly ImmutableStringBuilderRecord _immutableStringBuilderRecord;
	private readonly ImmutableStringBuilderRecordKey _immutableStringBuilderRecordKey;

	public DefaultPlainTextToken(KeyDownEventRecord keyDownEventRecord)
		: this(PlainTextTokenKey.NewPlainTextTokenKey(), 0, SequenceKey.NewSequenceKey())
	{
		_immutableStringBuilderRecord = new();

		_immutableStringBuilderRecordKey = _immutableStringBuilderRecord
			.Append(keyDownEventRecord.Key);
	}	
	
	public DefaultPlainTextToken(KeyDownEventRecord keyDownEventRecord,
		DefaultPlainTextToken otherDefaultPlainTextToken)
		: this(otherDefaultPlainTextToken.PlainTextTokenKey, otherDefaultPlainTextToken.IndexInPlainText + 1, SequenceKey.NewSequenceKey())
	{
		_immutableStringBuilderRecord = otherDefaultPlainTextToken._immutableStringBuilderRecord;
		
		_immutableStringBuilderRecordKey = _immutableStringBuilderRecord
			.Append(keyDownEventRecord.Key);
	}	
	
	public override PlainTextTokenKind PlainTextTokenKind => PlainTextTokenKind.Default;
	public override string ToPlainText => _immutableStringBuilderRecord
		.GetString(_immutableStringBuilderRecordKey);
}