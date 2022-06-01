using FluxorExperiments.ClassLibrary.FeatureStateContainer;
using FluxorExperiments.ClassLibrary.Sequence;
using System.Collections.Immutable;

namespace FluxorExperiments.ClassLibrary.PlainTextEditor;

public record PlainTextRow
	: FeatureStateContainerRecord<PlainTextRow, PlainTextTokenKey, PlainTextTokenBase>, 
		IManyFeatureState<PlainTextRowKey, PlainTextRow>
{
	public SequenceKeyRecord SequenceKeyRecord { get; }
	public PlainTextRowKey KeyRecord { get; set; }
	
	public PlainTextRow ConstructDeepClone() => this with { };
}