using FluxorExperiments.ClassLibrary.Keyboard;
using FluxorExperiments.ClassLibrary.KeyDownEvent;
using FluxorExperiments.ClassLibrary.Sequence;

namespace FluxorExperiments.ClassLibrary.PlainTextEditor;

public record WhitespacePlainTextToken(PlainTextTokenKey PlainTextTokenKey, int? IndexInPlainText, SequenceKey SequenceKey) 
	: PlainTextTokenBase(PlainTextTokenKey, IndexInPlainText, SequenceKey)
{
	private readonly char _whitespaceCharacter;	
	
	public WhitespacePlainTextToken(KeyDownEventRecord keyDownEventRecord)
		: this(PlainTextTokenKey.NewPlainTextTokenKey(), 0, SequenceKey.NewSequenceKey())
	{
		_whitespaceCharacter = keyDownEventRecord.Code switch {
			KeyboardFacts.WhitespaceKeys.SPACE_CODE => ' ',
			KeyboardFacts.WhitespaceKeys.TAB_CODE => '\t',
			_ => throw new ApplicationException($"The whitespace with Code: '{keyDownEventRecord.Code}' was " +
			                                    $"not found in the {nameof(KeyboardFacts.WhitespaceKeys)} constants.")
		};
	}
	
	public override PlainTextTokenKind PlainTextTokenKind => PlainTextTokenKind.Whitespace;
	public override string ToPlainText => _whitespaceCharacter.ToString();
}