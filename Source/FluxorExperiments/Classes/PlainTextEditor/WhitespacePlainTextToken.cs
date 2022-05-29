﻿using FluxorExperiments.Classes.Keyboard;
using FluxorExperiments.Classes.KeyDownEvent;
using FluxorExperiments.Classes.Sequence;

namespace FluxorExperiments.Classes.PlainTextEditor;

public record WhitespacePlainTextToken(PlainTextTokenKey PlainTextTokenKey, SequenceKey SequenceKey) 
	: PlainTextTokenBase(PlainTextTokenKey, SequenceKey)
{
	private readonly char _whitespaceCharacter;	
	
	public WhitespacePlainTextToken(KeyDownEventRecord keyDownEventRecord)
		: this(PlainTextTokenKey.NewPlainTextTokenKey(), SequenceKey.NewSequenceKey())
	{
		_whitespaceCharacter = keyDownEventRecord.Code switch {
			KeyboardFacts.WhitespaceKeys.Space => ' ',
			KeyboardFacts.WhitespaceKeys.Tab => '\t',
			_ => throw new ApplicationException($"The whitespace with Code: '{keyDownEventRecord.Code}' was " +
			                                    $"not found in the {nameof(KeyboardFacts.WhitespaceKeys)} constants.")
		};
	}
	
	public override PlainTextTokenKind PlainTextTokenKind => PlainTextTokenKind.Whitespace;
	public override string ToPlainText => _whitespaceCharacter.ToString();
}