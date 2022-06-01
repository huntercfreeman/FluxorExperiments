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

	[Parameter, EditorRequired]
	public PlainTextRow PlainTextRow { get; set; } = null!;

	private SequenceKeyRecord? _previouslyRenderedWithPlainTextRowSequenceKey;
	private int _rerenderCount;

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
}