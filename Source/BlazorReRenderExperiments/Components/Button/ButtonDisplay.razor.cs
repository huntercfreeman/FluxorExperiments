using Microsoft.AspNetCore.Components;

namespace BlazorReRenderExperiments.Components.Button;

public partial class ButtonDisplay : ComponentBase
{
	[Parameter, EditorRequired]
	public RenderFragment ChildContent { get; set; } = null!;
	[Parameter, EditorRequired]
	public EventCallback OnClickEventCallback { get; set; }

	protected override void OnParametersSet()
	{
		if (!OnClickEventCallback.HasDelegate)
		{
			throw new ApplicationException($"The required Blazor Parameter named: " +
			                               $"{nameof(OnClickEventCallback)} was not provided.");
		}
		
		base.OnParametersSet();
	}

	private void FireOnClickEventCallback()
	{
		OnClickEventCallback.InvokeAsync();
	}
}