using Microsoft.AspNetCore.Components;

namespace BlazorReRenderExperiments.Components.RenderCounter;

public partial class RenderCounterHelperDisplay : ComponentBase
{
	private int _renderCount;

	protected override void OnParametersSet()
	{
		base.OnParametersSet();
	}

	protected override bool ShouldRender()
	{
		return base.ShouldRender();
	}

	public async Task UpdateUi(int renderCount)
	{
		_renderCount = renderCount;
		await InvokeAsync(StateHasChanged);
	}
}