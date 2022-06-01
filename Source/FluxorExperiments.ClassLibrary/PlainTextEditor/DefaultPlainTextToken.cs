using FluxorExperiments.ClassLibrary.ImmutableStringBuilder;
using FluxorExperiments.ClassLibrary.Keyboard;
using FluxorExperiments.ClassLibrary.KeyDownEvent;

namespace FluxorExperiments.ClassLibrary.PlainTextEditor;

public record DefaultPlainTextToken(int? IndexInPlainText) 
	: PlainTextTokenBase(IndexInPlainText)
{
	// TODO: DefaultPlainTextToken needs to be immutable look into perhaps Span<T>?
	private readonly ImmutableStringBuilderRecord _immutableStringBuilderRecord;
	private ImmutableStringBuilderRecordKey _immutableStringBuilderRecordKey;

	public DefaultPlainTextToken(KeyDownEventRecord keyDownEventRecord)
		: this(0)
	{
		_immutableStringBuilderRecord = new();

		_immutableStringBuilderRecord.Insert(0, keyDownEventRecord.Key);

		_immutableStringBuilderRecordKey = new ImmutableStringBuilderRecordKey(1);
	}	
	
	public DefaultPlainTextToken(string initialString)
		: this(0)
	{
		_immutableStringBuilderRecord = new();

		_immutableStringBuilderRecord.Insert(0, initialString);

		_immutableStringBuilderRecordKey = new ImmutableStringBuilderRecordKey(initialString.Length);
	}	
	
	public DefaultPlainTextToken(KeyDownEventRecord keyDownEventRecord,
		DefaultPlainTextToken otherDefaultPlainTextToken)
		: this(otherDefaultPlainTextToken.IndexInPlainText + 1)
	{
		KeyRecord = otherDefaultPlainTextToken.KeyRecord;
		
		if (keyDownEventRecord.Key == KeyboardFacts.MetaKeys.BACKSPACE)
		{
			_immutableStringBuilderRecord = otherDefaultPlainTextToken._immutableStringBuilderRecord;

			_immutableStringBuilderRecord.RemoveAt(otherDefaultPlainTextToken.IndexInPlainText.Value);
			
			_immutableStringBuilderRecordKey = 
				new ImmutableStringBuilderRecordKey(
					otherDefaultPlainTextToken._immutableStringBuilderRecordKey.Length - 1);

			IndexInPlainText = otherDefaultPlainTextToken.IndexInPlainText - 1;
		}
		else
		{
			_immutableStringBuilderRecord = otherDefaultPlainTextToken._immutableStringBuilderRecord;
			
			_immutableStringBuilderRecord.Insert(otherDefaultPlainTextToken.IndexInPlainText.Value + 1,
				keyDownEventRecord.Key);

			_immutableStringBuilderRecordKey = 
				new ImmutableStringBuilderRecordKey(otherDefaultPlainTextToken._immutableStringBuilderRecordKey.Length + 1);
		}
	}
	
	public DefaultPlainTextToken(DefaultPlainTextToken tokenFirst,
		DefaultPlainTextToken tokenSecond)
		: this(tokenFirst.IndexInPlainText)
	{
		 _immutableStringBuilderRecord = tokenFirst._immutableStringBuilderRecord;
		 
		 _immutableStringBuilderRecord.Insert(tokenFirst._immutableStringBuilderRecordKey.Length,
			 tokenSecond.ToPlainText);

		 _immutableStringBuilderRecordKey = 
			 new ImmutableStringBuilderRecordKey(tokenFirst._immutableStringBuilderRecordKey.Length + 
			                                     tokenSecond._immutableStringBuilderRecordKey.Length);
	}
	
	public override PlainTextTokenKind PlainTextTokenKind => PlainTextTokenKind.Default;
	public override string ToPlainText => _immutableStringBuilderRecord
		.GetString(_immutableStringBuilderRecordKey);
}