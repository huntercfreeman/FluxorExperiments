using Fluxor;
using Fluxor.Blazor.Web.Components;
using FluxorExperiments.ClassLibrary.Store.PlainTextEditor;
using Microsoft.AspNetCore.Components;

namespace FluxorExperiments.RazorClassLibrary.PlainTextEditor;

public partial class CharacterDisplay : FluxorComponent
{
	[Inject]
	private IState<PlainTextEditorState> PlainTextEditorState { get; set; } = null!;
	[Inject]
	private IDispatcher Dispatcher { get; set; } = null!;
	
	[CascadingParameter(Name=nameof(RowIndex))]
	public int RowIndex { get; set; }
	[CascadingParameter(Name=nameof(PlainTextTokenKeyIndex))]
	public int PlainTextTokenKeyIndex { get; set; }
	[CascadingParameter(Name=nameof(PreviousRowsColumnCount))]
	public int PreviousRowsColumnCount { get; set; }
	[CascadingParameter(Name=nameof(CurrentRowPreviousTokensColumnCount))]
	public int CurrentRowPreviousTokensColumnCount { get; set; }
	
	[Parameter, EditorRequired]
	public bool ShouldDisplayCursor { get; set; }
	/// <summary>
	/// Some html characters look like: {AMPERSAND_CHARACTER}nbsp; for non-breaking-space for example.
	///
	/// Therefore a string type is needed for the parameter.
	/// </summary>
	[Parameter, EditorRequired]
	public string Character { get; set; } = null!;
	[Parameter, EditorRequired]
	public int CharacterIndex { get; set; }

	private string IsSelectedCssClass => PlainTextEditorState.Value.SelectionSpanRecord is not null &&
	                                     (PlainTextEditorState.Value.SelectionSpanRecord
		                                      .InclusiveStartingDocumentTextIndex ==
	                                      (PreviousRowsColumnCount + CurrentRowPreviousTokensColumnCount +
	                                       CharacterIndex))
		? "fe_is-selected"
		: "";

	private void DispatchPlainTextEditorCharacterOnClickAction()
	{
		var action = new PlainTextEditorCharacterOnClickAction(RowIndex, 
			PlainTextTokenKeyIndex,
			CharacterIndex);
		
		Dispatcher.Dispatch(action);
	}
}