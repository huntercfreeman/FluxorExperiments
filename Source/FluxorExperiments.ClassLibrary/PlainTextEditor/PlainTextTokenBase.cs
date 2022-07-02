﻿using FluxorExperiments.ClassLibrary.FeatureStateContainer;

namespace FluxorExperiments.ClassLibrary.PlainTextEditor;

public abstract record PlainTextTokenBase(int? IndexInPlainText)
	: IManyFeatureState<PlainTextTokenKey, PlainTextTokenBase>
{
	public abstract PlainTextTokenKind PlainTextTokenKind { get; }
	public abstract ReadOnlySpan<char> AsPlainTextSpan { get; }
	public SequenceKeyRecord SequenceKeyRecord { get; init; } = new(Guid.NewGuid());
	public PlainTextTokenKey KeyRecord { get; init; } = new(Guid.NewGuid());
	
	public PlainTextTokenBase ConstructDeepClone() => this with 
	{
		SequenceKeyRecord = new SequenceKeyRecord(Guid.NewGuid())
	};
}