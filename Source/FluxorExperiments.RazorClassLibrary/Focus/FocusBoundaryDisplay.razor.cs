using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;
using Fluxor;
using Fluxor.Blazor.Web.Components;
using Microsoft.JSInterop;

namespace FluxorExperiments.RazorClassLibrary.Focus;

public partial class FocusBoundaryDisplay : FluxorComponent
{
	[Inject]
	public IJSRuntime JsRuntime { get; set; } = null!;
	
	[Parameter, EditorRequired]
	public RenderFragment ChildContent { get; set; } = null!;
	
	[Parameter]
	public string? Class { get; set; }

	[Parameter]
	public bool InitiallySetFocusOnAfterRender { get; set; } = true;

	private FocusTrapDisplay? _focusTrapDisplay = null!;
	
	public bool FocusTrapIsFocused;
	
	protected override void OnAfterRender(bool firstRender)
	{
		if (firstRender && InitiallySetFocusOnAfterRender)
		{
			FireFocusIn();
		}
		
		base.OnAfterRender(firstRender);
	}

	public void FireFocusIn()
	{
		try
		{
			if(_focusTrapDisplay is not null)
				_focusTrapDisplay.GetFocusTrapElementReference().FocusAsync();
		}
		catch (Microsoft.JSInterop.JSException)
		{
			// Caused when calling:
			// await _focusTrap.FocusAsync();
			// After component is no longer rendered
		}
	}
	
	public void OnFocusTrapFocusIn()
	{
		JsRuntime.InvokeVoidAsync("fluxorExperiments.setOnKeyDownEventProviderIsActive",
			true);
		
		FocusTrapIsFocused = true;
		
		InvokeAsync(StateHasChanged);
	}
	
	public void OnFocusTrapFocusOut()
	{
		JsRuntime.InvokeVoidAsync("fluxorExperiments.setOnKeyDownEventProviderIsActive",
			false);
		
		FocusTrapIsFocused = false;
		
		InvokeAsync(StateHasChanged);
	}
}
