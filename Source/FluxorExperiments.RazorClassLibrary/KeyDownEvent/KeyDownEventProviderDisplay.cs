using Fluxor;
using FluxorExperiments.ClassLibrary.Clipboard;
using FluxorExperiments.ClassLibrary.KeyDownEvent;
using FluxorExperiments.ClassLibrary.Store.KeyDownEvent;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FluxorExperiments.RazorClassLibrary.KeyDownEvent;

public partial class KeyDownEventProviderDisplay : ComponentBase
{
	[Inject]
	private IJSRuntime JsRuntime { get; set; } = null!;
	[Inject]
	private IClipboardProvider ClipboardProvider { get; set; } = null!;
	[Inject]
	private IDispatcher Dispatcher { get; set; } = null!;

	protected override Task OnAfterRenderAsync(bool firstRender)
	{
		if(firstRender)
		{
			JsRuntime.InvokeVoidAsync("fluxorExperiments.initializeOnKeyDownEventProvider",
				DotNetObjectReference.Create(this)).AsTask();
		}

		return base.OnAfterRenderAsync(firstRender);
	}

	[JSInvokable]
	public void DispatchOnKeyDownEventAction(KeyDownEventRecord keyDownEventRecord)
	{
		bool handledControlModifiedKeyPress = false;

		if (keyDownEventRecord.CtrlWasPressed)
		{
			if (keyDownEventRecord.Key == "c")
			{
				Dispatcher.Dispatch(new CopyEventAction(keyDownEventRecord, ClipboardProvider));
				handledControlModifiedKeyPress = true;
			}
			else if (keyDownEventRecord.Key == "v")
			{
				Dispatcher.Dispatch(new PasteEventAction(keyDownEventRecord, ClipboardProvider));
				handledControlModifiedKeyPress = true;
			}
		}

		if (!handledControlModifiedKeyPress)
			Dispatcher.Dispatch(new KeyDownEventAction(keyDownEventRecord));
	}
}