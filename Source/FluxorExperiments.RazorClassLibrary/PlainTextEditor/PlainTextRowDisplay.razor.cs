using Fluxor;
using FluxorExperiments.ClassLibrary.FeatureStateContainer;
using FluxorExperiments.ClassLibrary.PlainTextEditor;
using FluxorExperiments.ClassLibrary.Store.PlainTextEditor;
using Microsoft.AspNetCore.Components;

namespace FluxorExperiments.RazorClassLibrary.PlainTextEditor;

public partial class PlainTextRowDisplay : ComponentBase
{
	[Inject]
	private IState<PlainTextEditorState> PlainTextEditorState { get; set; } = null!;
	[Inject]
	private IDispatcher Dispatcher { get; set; } = null!;

	[Parameter, EditorRequired]
	public PlainTextRow PlainTextRow { get; set; } = null!;
	[Parameter, EditorRequired]
	public int PlainTextRowIndex { get; set; }

	private SequenceKeyRecord? _previouslyRenderedWithPlainTextRowSequenceKey;
	private int _rerenderCount;

	private string IsActiveRowCssClass => PlainTextEditorState.Value.CurrentRowIndex == PlainTextRowIndex
		? "fe_active"
		: "";

	protected override void OnAfterRender(bool firstRender)
	{
		_rerenderCount++;

		base.OnAfterRender(firstRender);
	}

	protected override bool ShouldRender()
	{
		var shouldRender = _previouslyRenderedWithPlainTextRowSequenceKey is null ||
		                   _previouslyRenderedWithPlainTextRowSequenceKey != PlainTextRow.SequenceKeyRecord;

		_previouslyRenderedWithPlainTextRowSequenceKey = PlainTextRow.SequenceKeyRecord;

		return shouldRender;
	}
	
	private void DispatchPlainTextEditorCharacterOnClickAction()
	{
		var action = new PlainTextEditorCharacterOnClickAction(PlainTextRowIndex, 
			PlainTextRow.Count - 1,
			PlainTextRow[^1].ToPlainText.Length - 1);
		
		Dispatcher.Dispatch(action);
	}
}