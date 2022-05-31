using Fluxor;
using FluxorExperiments.ClassLibrary.Store.KeyDownEvent;

namespace FluxorExperiments.ClassLibrary.Store.PlainTextEditor;

public class PlainTextEditorReducer
{
	[ReducerMethod]
	public static PlainTextEditorState ReducePlainTextEditorKeyDownEventAction(PlainTextEditorState previousPlainTextEditorState,
		PlainTextEditorKeyDownEventAction plainTextEditorKeyDownEventAction)
	{
		Console.WriteLine($"Key: {plainTextEditorKeyDownEventAction.KeyDownEventRecord.Key} was pressed.");

		return previousPlainTextEditorState.GetNextState(plainTextEditorKeyDownEventAction.KeyDownEventRecord);
	}
	
	[ReducerMethod]
	public static PlainTextEditorState ReducePlainTextEditorCharacterOnClickAction(PlainTextEditorState previousPlainTextEditorState,
		PlainTextEditorCharacterOnClickAction plainTextEditorCharacterOnClickAction)
	{
		return previousPlainTextEditorState.GetNextState(plainTextEditorCharacterOnClickAction.RowIndex,
			plainTextEditorCharacterOnClickAction.PlainTextTokenKeyIndex,
			plainTextEditorCharacterOnClickAction.CharacterIndex);
	}
	
	[ReducerMethod]
	public static PlainTextEditorState ReducePlainTextEditorCopyAction(PlainTextEditorState previousPlainTextEditorState,
		PlainTextEditorCopyAction plainTextEditorCopyAction)
	{
		_ = Task.Run(async () => 
			await plainTextEditorCopyAction.ClipboardProvider.SetClipboard(previousPlainTextEditorState.EntireDocumentToPlainText));

		return previousPlainTextEditorState;
	}
}