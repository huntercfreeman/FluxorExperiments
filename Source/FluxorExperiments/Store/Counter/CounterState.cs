using Fluxor;

namespace FluxorExperiments.Store.Counter;

// Ample usage of #region as this project is for experiments
// not production code.

#region FeatureStates
[FeatureState]
public record CounterState(int Count)
{
	public CounterState() : this(0)
	{
		
	}
}
#endregion

#region Actions
public record IncrementCounterAction();
#endregion

#region Reducers
public class CounterReducer
{
	[ReducerMethod(typeof(IncrementCounterAction))]
	public static CounterState ReduceIncrementCounterAction(CounterState previousCounterState)
	{
		return previousCounterState with {
			Count = previousCounterState.Count + 1
		};
	}
}
#endregion