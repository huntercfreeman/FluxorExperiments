using FluxorExperiments.ClassLibrary.Sequence;

namespace FluxorExperiments.ClassLibrary.PlainTextEditor;

public abstract record PlainTextTokenBase(PlainTextTokenKey PlainTextTokenKey, int? IndexInPlainText, SequenceKey SequenceKey)
{
	public abstract PlainTextTokenKind PlainTextTokenKind { get; }
	public abstract string ToPlainText { get; }
}