using Fluxor;
using Fluxor.Blazor.Web.Components;
using FluxorExperiments.ClassLibrary.PlainTextEditor;
using FluxorExperiments.ClassLibrary.Store.PlainTextEditor;
using FluxorExperiments.RazorClassLibrary.Focus;
using Microsoft.AspNetCore.Components;

namespace FluxorExperiments.RazorClassLibrary.PlainTextEditor;

public partial class CharacterDisplay
{
	[Inject]
	private IDispatcher Dispatcher { get; set; } = null!;

	[CascadingParameter]
	public FocusBoundaryDisplay FocusBoundaryDisplay { get; set; } = null!;
	[CascadingParameter]
	public IState<PlainTextEditorState> PlainTextEditorState { get; set; } = null!;
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

	private string IsSelectedCssClass => GetIsSelectedCss();

	private void DispatchPlainTextEditorCharacterOnClickAction()
	{
		var action = new PlainTextEditorCharacterOnClickAction(RowIndex, 
			PlainTextTokenKeyIndex,
			CharacterIndex);
		
		Dispatcher.Dispatch(action);
		
		FocusBoundaryDisplay.FireFocusIn();
	}

	private string GetIsSelectedCss()
	{
		var selectionSpanRecord = PlainTextEditorState.Value.SelectionSpanRecord;
		
		if (selectionSpanRecord is null)
		{
			return string.Empty;
		}
		
		var indexOfCharacterRelativeToDocument = PreviousRowsColumnCount +
		                                         CurrentRowPreviousTokensColumnCount +
		                                         CharacterIndex;

		int inclusiveMinimumIndexForSelectedCss;
		int inclusiveMaximumIndexForSelectedCss;

		if (selectionSpanRecord.Value.InitialDirectionBinding == SelectionDirectionBinding.Left)
		{
			inclusiveMinimumIndexForSelectedCss = selectionSpanRecord.Value.InclusiveStartingColumnIndexRelativeToDocument +
                                                  			selectionSpanRecord.Value.OffsetDisplacement;
			inclusiveMaximumIndexForSelectedCss = selectionSpanRecord.Value.InclusiveStartingColumnIndexRelativeToDocument;
		}
		else
		{
			inclusiveMinimumIndexForSelectedCss = selectionSpanRecord.Value.InclusiveStartingColumnIndexRelativeToDocument;
			inclusiveMaximumIndexForSelectedCss = selectionSpanRecord.Value.InclusiveStartingColumnIndexRelativeToDocument +
			                                      selectionSpanRecord.Value.OffsetDisplacement;
		}
		
		if (indexOfCharacterRelativeToDocument >= inclusiveMinimumIndexForSelectedCss &&
		    indexOfCharacterRelativeToDocument <= inclusiveMaximumIndexForSelectedCss)
		{
			return "fe_is-selected";
		}

		return string.Empty;
	}
}