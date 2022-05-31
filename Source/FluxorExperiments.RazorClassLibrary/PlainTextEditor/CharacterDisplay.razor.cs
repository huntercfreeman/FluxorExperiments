using Fluxor;
using Fluxor.Blazor.Web.Components;
using FluxorExperiments.ClassLibrary.PlainTextEditor;
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

	private string IsSelectedCssClass => GetIsSelectedCss();

	private void DispatchPlainTextEditorCharacterOnClickAction()
	{
		var action = new PlainTextEditorCharacterOnClickAction(RowIndex, 
			PlainTextTokenKeyIndex,
			CharacterIndex);
		
		Dispatcher.Dispatch(action);
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

		if (selectionSpanRecord.InitialDirectionBinding == SelectionDirectionBinding.Left)
		{
			inclusiveMinimumIndexForSelectedCss = selectionSpanRecord.InclusiveStartingColumnIndexRelativeToDocument +
                                                  			selectionSpanRecord.OffsetDisplacement;
			inclusiveMaximumIndexForSelectedCss = selectionSpanRecord.InclusiveStartingColumnIndexRelativeToDocument;
		}
		else
		{
			inclusiveMinimumIndexForSelectedCss = selectionSpanRecord.InclusiveStartingColumnIndexRelativeToDocument;
			inclusiveMaximumIndexForSelectedCss = selectionSpanRecord.InclusiveStartingColumnIndexRelativeToDocument +
			                                      selectionSpanRecord.OffsetDisplacement;
		}
		
		if (indexOfCharacterRelativeToDocument >= inclusiveMinimumIndexForSelectedCss &&
		    indexOfCharacterRelativeToDocument <= inclusiveMaximumIndexForSelectedCss)
		{
			return "fe_is-selected";
		}

		return string.Empty;
	}
}