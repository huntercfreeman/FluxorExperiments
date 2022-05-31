using Fluxor;
using FluxorExperiments.ClassLibrary.KeyDownEvent;

namespace FluxorExperiments.ClassLibrary.Store.KeyDownEvent;

[FeatureState]
public record class KeyDownEventState(KeyDownEventRecord? OnKeyDownEventRecord)
{
	public KeyDownEventState() : this(default(KeyDownEventRecord))
	{

	}
}