using Fluxor;
using FluxorExperiments.ClassLibrary.PlainTextEditor;
using FluxorExperiments.ClassLibrary.Store.PlainTextEditor;
using Microsoft.AspNetCore.Components;

namespace FluxorExperiments.RazorClassLibrary.PlainTextEditor;

public partial class PlainTextTokenDisplay : ComponentBase
{
	[Inject]
	private IState<PlainTextEditorState> PlainTextEditorState { get; set; } = null!;

	[Parameter, EditorRequired]
	public PlainTextRow PlainTextRow { get; set; } = null!;
	[Parameter, EditorRequired]
	public PlainTextTokenKey PlainTextTokenKey { get; set; } = null!;
	
	private PlainTextTokenBase? _cachedPlainTextToken;
	private int _rerenderCount;

	protected override void OnAfterRender(bool firstRender)
	{
		_rerenderCount++;
		
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

	private bool GetShouldDisplayCursor()
	{
		return PlainTextEditorState.Value.CurrentPlainTextToken.PlainTextTokenKey ==
		       PlainTextTokenKey;
	}
}