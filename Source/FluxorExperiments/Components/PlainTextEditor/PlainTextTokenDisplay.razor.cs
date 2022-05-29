using Fluxor;
using FluxorExperiments.Classes.PlainTextEditor;
using FluxorExperiments.Store.PlainTextEditor;
using Microsoft.AspNetCore.Components;

namespace FluxorExperiments.Components.PlainTextEditor;

public partial class PlainTextTokenDisplay : ComponentBase
{
	[Inject]
	private IState<PlainTextEditorState> PlainTextEditorState { get; set; } = null!;

	[Parameter, EditorRequired]
	public PlainTextRow PlainTextRow { get; set; } = null!;
	[Parameter, EditorRequired]
	public PlainTextTokenKey PlainTextTokenKey { get; set; } = null!;
	
	private PlainTextTokenBase? _cachedPlainTextToken;

	protected override void OnAfterRender(bool firstRender)
	{
		if (firstRender)
		{
			StateHasChanged();
		}
		
		base.OnAfterRender(firstRender);
	}

	protected override bool ShouldRender()
	{
		try
		{
			var currentPlainTextToken = PlainTextRow.LookupPlainTextToken(PlainTextTokenKey);

			var shouldRender = _cachedPlainTextToken is null || 
			       _cachedPlainTextToken.SequenceKey != currentPlainTextToken.SequenceKey;

			_cachedPlainTextToken = currentPlainTextToken;

			return shouldRender;
		}
		catch (KeyNotFoundException)
		{
			return false;
		}
	}
}