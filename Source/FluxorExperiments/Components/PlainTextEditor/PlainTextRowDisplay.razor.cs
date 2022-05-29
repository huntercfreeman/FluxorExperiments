using Fluxor;
using FluxorExperiments.Classes.PlainTextEditor;
using FluxorExperiments.Store.PlainTextEditor;
using Microsoft.AspNetCore.Components;

namespace FluxorExperiments.Components.PlainTextEditor;

public partial class PlainTextRowDisplay : ComponentBase
{
	[Inject]
	private IState<PlainTextEditorState> PlainTextEditorState { get; set; } = null!;
	
	[Parameter, EditorRequired]
	public PlainTextRowKey PlainTextRowKey { get; set; } = null!;

	private PlainTextRow? _cachedPlainTextRow;
	private int _rerenderCount;

	protected override void OnAfterRender(bool firstRender)
	{
		_rerenderCount++;
		
		if (firstRender)
		{
			// _cachedPlainTextRow is not guaranteed to have had data on first render.
			// and I think an atomic race condition is occurring where the _cachedPlainTextRow
			// becomes available to cache but the component doesn't end up re-rendering
			//
			// I do not believe ShouldRender() is called on the first render is possibly the actual issue
			StateHasChanged();
		}
		
		base.OnAfterRender(firstRender);
	}

	protected override bool ShouldRender()
	{
		try
		{
			var currentPlainTextRow = PlainTextEditorState.Value.LookupPlainTextRow(PlainTextRowKey);

			var shouldRender = _cachedPlainTextRow is null || 
			       _cachedPlainTextRow.SequenceKey != currentPlainTextRow.SequenceKey;

			_cachedPlainTextRow = currentPlainTextRow;

			return shouldRender;
		}
		catch (KeyNotFoundException)
		{
			return false;
		}
	}
}