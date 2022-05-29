using Fluxor;
using FluxorExperiments.Classes.KeyDownEvent;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FluxorExperiments.Components.KeyDownEvent;

public partial class KeyDownEventProviderDisplay : ComponentBase
{
	[Inject]
	private IJSRuntime JsRuntime { get; set; } = null!;
	[Inject]
	private IDispatcher Dispatcher { get; set; } = null!;

	protected override Task OnAfterRenderAsync(bool firstRender)
	{
		if(firstRender)
		{
			JsRuntime.InvokeVoidAsync("fluxorExperiments.initializeOnKeyDownEventProvider",
				DotNetObjectReference.Create(this));
		}

		return base.OnAfterRenderAsync(firstRender);
	}

	[JSInvokable]
	public void DispatchOnKeyDownEventAction(KeyDownEventRecord onKeyDownEventRecord)
	{
		var action = new KeyDownEventAction(onKeyDownEventRecord);

		Dispatcher.Dispatch(action);
	}
}