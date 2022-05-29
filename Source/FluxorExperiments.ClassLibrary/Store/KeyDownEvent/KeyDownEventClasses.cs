using Fluxor;
using FluxorExperiments.ClassLibrary.KeyDownEvent;

namespace FluxorExperiments.ClassLibrary.Store.KeyDownEvent;

public record class KeyDownEventAction(KeyDownEventRecord OnKeyDownEventRecord);

public class KeyDownEventReducers
{
	[ReducerMethod]
	public static KeyDownEventState ReduceOnKeyDownEventAction(KeyDownEventState previousKeyDownEventState,
		KeyDownEventAction onKeyDownEventAction)
	{
		return new KeyDownEventState(onKeyDownEventAction.OnKeyDownEventRecord);
	}
}

[FeatureState]
public record class KeyDownEventState(KeyDownEventRecord? OnKeyDownEventRecord)
{
	public KeyDownEventState() : this(default(KeyDownEventRecord))
	{

	}
}