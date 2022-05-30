using FluxorExperiments.ClassLibrary.ImmutableStringBuilder;
using FluxorExperiments.ClassLibrary.Keyboard;
using FluxorExperiments.ClassLibrary.KeyDownEvent;
using FluxorExperiments.ClassLibrary.Sequence;

namespace FluxorExperiments.ClassLibrary.PlainTextEditor;

public record DefaultPlainTextToken(PlainTextTokenKey PlainTextTokenKey, int? IndexInPlainText, SequenceKey SequenceKey) 
	: PlainTextTokenBase(PlainTextTokenKey, IndexInPlainText, SequenceKey)
{
	// TODO: DefaultPlainTextToken needs to be immutable look into perhaps Span<T>?
	private readonly ImmutableStringBuilderRecord _immutableStringBuilderRecord;
	private ImmutableStringBuilderRecordKey _immutableStringBuilderRecordKey;

	public DefaultPlainTextToken(KeyDownEventRecord keyDownEventRecord)
		: this(PlainTextTokenKey.NewPlainTextTokenKey(), 0, SequenceKey.NewSequenceKey())
	{
		_immutableStringBuilderRecord = new();

		_immutableStringBuilderRecord.Insert(0, keyDownEventRecord.Key);

		_immutableStringBuilderRecordKey = new ImmutableStringBuilderRecordKey(1);
	}	
	
	public DefaultPlainTextToken(KeyDownEventRecord keyDownEventRecord,
		DefaultPlainTextToken otherDefaultPlainTextToken)
		: this(otherDefaultPlainTextToken.PlainTextTokenKey, otherDefaultPlainTextToken.IndexInPlainText + 1, SequenceKey.NewSequenceKey())
	{
		if (keyDownEventRecord.Key == KeyboardFacts.MetaKeys.BACKSPACE)
		{
			_immutableStringBuilderRecord = otherDefaultPlainTextToken._immutableStringBuilderRecord;
			
			_immutableStringBuilderRecordKey = 
				new ImmutableStringBuilderRecordKey(
					otherDefaultPlainTextToken._immutableStringBuilderRecordKey.Length - 1);

			IndexInPlainText = otherDefaultPlainTextToken.IndexInPlainText - 1;
		}
		else
		{
			_immutableStringBuilderRecord = otherDefaultPlainTextToken._immutableStringBuilderRecord;
			
			_immutableStringBuilderRecord.Insert(otherDefaultPlainTextToken._immutableStringBuilderRecordKey.Length,
				keyDownEventRecord.Key);

			_immutableStringBuilderRecordKey = 
				new ImmutableStringBuilderRecordKey(otherDefaultPlainTextToken._immutableStringBuilderRecordKey.Length + 1);
		}
	}
	
	public override PlainTextTokenKind PlainTextTokenKind => PlainTextTokenKind.Default;
	public override string ToPlainText => _immutableStringBuilderRecord
		.GetString(_immutableStringBuilderRecordKey);
}