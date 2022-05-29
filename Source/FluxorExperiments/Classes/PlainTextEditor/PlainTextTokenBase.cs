using FluxorExperiments.Classes.Sequence;

namespace FluxorExperiments.Classes.PlainTextEditor;

public abstract record PlainTextTokenBase(PlainTextTokenKey PlainTextTokenKey, SequenceKey SequenceKey)
{
	public abstract PlainTextTokenKind PlainTextTokenKind { get; }
	public abstract string ToPlainText { get; }
}