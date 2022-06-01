using FluxorExperiments.ClassLibrary.FeatureStateContainer;
using FluxorExperiments.ClassLibrary.Sequence;

namespace FluxorExperiments.ClassLibrary.PlainTextEditor;

public abstract record PlainTextTokenBase(int? IndexInPlainText)
	: IManyFeatureState<PlainTextTokenKey, PlainTextTokenBase>
{
	public abstract PlainTextTokenKind PlainTextTokenKind { get; }
	public abstract string ToPlainText { get; }
	
	public SequenceKeyRecord SequenceKeyRecord { get; } = new();
	public PlainTextTokenKey KeyRecord { get; set; } = new();
	
	public PlainTextTokenBase ConstructDeepClone() => this with { };
}