using Microsoft.AspNetCore.Components;

namespace BlazorReRenderExperiments.Components.RenderCounter;

public partial class RenderCounterDisplay : ComponentBase
{
	private Guid _renderCounterDisplayId = Guid.NewGuid();
	private int _renderCount;

	private RenderCounterHelperDisplay _renderCounterHelperDisplay = null!;
	
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		_renderCount++;

		await _renderCounterHelperDisplay.UpdateUi(_renderCount);
		
		await base.OnAfterRenderAsync(firstRender);
	}

	protected override bool ShouldRender()
	{
		return base.ShouldRender();
	}
}