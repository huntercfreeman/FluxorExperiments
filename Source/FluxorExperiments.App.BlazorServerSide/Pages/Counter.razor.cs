using Fluxor;
using Fluxor.Blazor.Web.Components;
using FluxorExperiments.ClassLibrary.Store.Counter;
using Microsoft.AspNetCore.Components;

namespace FluxorExperiments.App.BlazorServerSide.Pages;

public partial class Counter : FluxorComponent
{
    [Inject]
    private IState<CounterState> CounterState { get; set; } = null!;
    [Inject]
    private IDispatcher Dispatcher { get; set; } = null!;

    private int _amountOfIncrementCounterActionsToDispatch;

    private void DispatchIncrementCounterActionOnClick()
    {
        var incrementCounterAction = new IncrementCounterAction();
        
        Dispatcher.Dispatch(incrementCounterAction);
    }
    
    private void DispatchVariableAmountOfIncrementCounterActionsOnClick()
    {
	    for (int i = 0; i < _amountOfIncrementCounterActionsToDispatch; i++)
	    {
			var incrementCounterAction = new IncrementCounterAction();
			
			Dispatcher.Dispatch(incrementCounterAction);
	    }
    }
}