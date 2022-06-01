using Fluxor;

namespace BlazorReRenderExperiments.Store.RenderCounter;

public class RenderCounterBoardReducer
{
	[ReducerMethod]
	public static RenderCounterBoardState ReduceRegisterRenderCounter(RenderCounterBoardState previousRenderCounterBoardState, 
		RegisterRenderCounterAction registerRenderCounterAction)
	{
		return previousRenderCounterBoardState
			.WithAdd(registerRenderCounterAction.RenderCounterRowRecordKey, 
				registerRenderCounterAction.RenderCounterRecordKey);
	}
	
	[ReducerMethod]
	public static RenderCounterBoardState ReduceUnregisterRenderCounter(RenderCounterBoardState previousRenderCounterBoardState, 
		UnregisterRenderCounterAction unregisterRenderCounterAction)
	{

		return previousRenderCounterBoardState
			.WithRemove(unregisterRenderCounterAction.RenderCounterRowRecordKey, 
				unregisterRenderCounterAction.RenderCounterRecordKey);
	}
	
	[ReducerMethod]
	public static RenderCounterBoardState ReduceIncrementRenderCounter(RenderCounterBoardState previousRenderCounterBoardState, 
		IncrementRenderCounterAction incrementRenderCounterAction)
	{

		return previousRenderCounterBoardState
			.WithReplace(incrementRenderCounterAction.RenderCounterRowRecordKey, 
				incrementRenderCounterAction.RenderCounterRecordKey);
	}
}