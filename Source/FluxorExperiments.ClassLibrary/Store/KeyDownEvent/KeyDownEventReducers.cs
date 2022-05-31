using Fluxor;

namespace FluxorExperiments.ClassLibrary.Store.KeyDownEvent;

public class KeyDownEventReducers
{
	[ReducerMethod]
	public static KeyDownEventState ReduceOnKeyDownEventAction(KeyDownEventState previousKeyDownEventState,
		KeyDownEventAction onKeyDownEventAction)
	{
		return new KeyDownEventState(onKeyDownEventAction.KeyDownEventRecord);
	}
}