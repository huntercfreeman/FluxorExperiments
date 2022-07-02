using Fluxor;
using FluxorExperiments.ClassLibrary.FeatureStateContainer;
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
	public PlainTextTokenBase PlainTextToken { get; set; } = null!;
	
	private int _rerenderCount;
	private SequenceKeyRecord? _previousRenderedWithPlainTextTokenSequenceKey;

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
		var shouldRender = _previousRenderedWithPlainTextTokenSequenceKey is null ||
						   !_previousRenderedWithPlainTextTokenSequenceKey.Equals(PlainTextToken.SequenceKeyRecord);


		_previousRenderedWithPlainTextTokenSequenceKey = PlainTextToken.SequenceKeyRecord;

		return shouldRender;
	}

	private bool GetShouldDisplayCursor()
	{
		return PlainTextEditorState.Value.CurrentPlainTextToken.KeyRecord ==
		       PlainTextToken.KeyRecord;
	}
}