﻿using FluxorExperiments.Classes.Sequence;

namespace FluxorExperiments.Classes.PlainTextEditor;

public record StartOfRowPlainTextToken(PlainTextTokenKey PlainTextTokenKey, int? IndexInContent, SequenceKey SequenceKey) 
	: PlainTextTokenBase(PlainTextTokenKey, IndexInContent, SequenceKey)
{
	public StartOfRowPlainTextToken()
		: this(PlainTextTokenKey.NewPlainTextTokenKey(), 0, SequenceKey.NewSequenceKey())
	{
		
	}
	
	public override PlainTextTokenKind PlainTextTokenKind => PlainTextTokenKind.StartOfRow;
	public override string ToPlainText => "\n";
}