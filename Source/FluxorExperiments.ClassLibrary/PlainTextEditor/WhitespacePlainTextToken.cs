using FluxorExperiments.ClassLibrary.Html;
using FluxorExperiments.ClassLibrary.Keyboard;
using FluxorExperiments.ClassLibrary.KeyDownEvent;

namespace FluxorExperiments.ClassLibrary.PlainTextEditor;

public record WhitespacePlainTextToken(int? IndexInPlainText) 
	: PlainTextTokenBase(IndexInPlainText)
{
	private const string SPACE_CODE = " ";
	private const string TAB_CODE = "\t";
	private readonly string _whitespaceCharacter = string.Empty;
	public WhitespacePlainTextToken(KeyDownEventRecord keyDownEventRecord)
		: this(0) => _whitespaceCharacter = keyDownEventRecord.Code switch {
			KeyboardFacts.WhitespaceKeys.SPACE_CODE => SPACE_CODE,
			KeyboardFacts.WhitespaceKeys.TAB_CODE => TAB_CODE,
			_ => throw new ApplicationException($"The whitespace with Code: '{keyDownEventRecord.Code}' was " +
												$"not found in the {nameof(KeyboardFacts.WhitespaceKeys)} constants.")
		};

	public override PlainTextTokenKind PlainTextTokenKind => PlainTextTokenKind.Whitespace;
	public override ReadOnlySpan<char> PlainTextSpan => _whitespaceCharacter.AsSpan();

	public override int PlanTextLength => 1;

	public override string PlainText => _whitespaceCharacter;
}