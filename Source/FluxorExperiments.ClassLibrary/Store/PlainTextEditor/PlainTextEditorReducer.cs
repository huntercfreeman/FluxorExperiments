using Fluxor;
using FluxorExperiments.ClassLibrary.Keyboard;
using FluxorExperiments.ClassLibrary.KeyDownEvent;
using FluxorExperiments.ClassLibrary.Store.KeyDownEvent;

namespace FluxorExperiments.ClassLibrary.Store.PlainTextEditor;

public class PlainTextEditorReducer
{
	[ReducerMethod]
	public static PlainTextEditorState ReducePlainTextEditorKeyDownEventAction(
		PlainTextEditorState previousPlainTextEditorState,
		KeyDownEventAction keyDownEventAction)
	{
		Console.WriteLine($"Key: {keyDownEventAction.KeyDownEventRecord.Key} was pressed.");

		return previousPlainTextEditorState.GetNextState(keyDownEventAction.KeyDownEventRecord);
	}
	
	[ReducerMethod]
	public static PlainTextEditorState ReduceBulkPlainTextEditorTextInsertAction(
		PlainTextEditorState previousPlainTextEditorState,
		BulkPlainTextEditorTextInsertAction pasteEventAction)
	{
		return previousPlainTextEditorState.GetNextState(pasteEventAction.Value);
	}

	[ReducerMethod]
	public static PlainTextEditorState ReducePlainTextEditorCharacterOnClickAction(
		PlainTextEditorState previousPlainTextEditorState,
		PlainTextEditorCharacterOnClickAction plainTextEditorCharacterOnClickAction)
	{
		return previousPlainTextEditorState.GetNextState(plainTextEditorCharacterOnClickAction.RowIndex,
			plainTextEditorCharacterOnClickAction.PlainTextTokenKeyIndex,
			plainTextEditorCharacterOnClickAction.CharacterIndex);
	}
	
	[ReducerMethod]
	public static PlainTextEditorState ReducePlainTextEditorCopyAction(
		PlainTextEditorState previousPlainTextEditorState,
		PlainTextEditorCopyAction plainTextEditorCopyAction)
	{
		_ = Task.Run(async () =>
			await plainTextEditorCopyAction.ClipboardProvider.SetClipboard(previousPlainTextEditorState
				.GetEntireDocumentToPlainText()));
	
		return previousPlainTextEditorState;
	}
}