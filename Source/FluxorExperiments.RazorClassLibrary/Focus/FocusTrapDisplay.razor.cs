using Microsoft.AspNetCore.Components;

namespace FluxorExperiments.RazorClassLibrary.Focus;

public partial class FocusTrapDisplay : ComponentBase
{
	private bool _initializedFocusTrap;
	private ElementReference _focusTrap;

	public ElementReference GetFocusTrapElementReference() => _focusTrap;

	[Parameter]
	public Action OnFocusTrapFocusIn { get; set; } = null!;
	[Parameter]
	public Action OnFocusTrapFocusOut { get; set; } = null!;

	protected override void OnAfterRender(bool firstRender)
	{
		if (firstRender)
		{
			_initializedFocusTrap = true;
		}

		base.OnAfterRender(firstRender);
	}

	protected override bool ShouldRender() => !_initializedFocusTrap;
}