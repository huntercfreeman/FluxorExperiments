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
		PlainTextEditorKeyDownEventAction plainTextEditorKeyDownEventAction)
	{
		Console.WriteLine($"Key: {plainTextEditorKeyDownEventAction.KeyDownEventRecord.Key} was pressed.");

		return previousPlainTextEditorState.GetNextState(plainTextEditorKeyDownEventAction.KeyDownEventRecord);
	}

	[ReducerMethod]
	public static PlainTextEditorState ReduceBulkPlainTextEditorTextInsertAction(
		PlainTextEditorState previousPlainTextEditorState,
		BulkPlainTextEditorTextInsertAction bulkPlainTextEditorTextInsertAction)
	{
		return previousPlainTextEditorState.GetNextState(bulkPlainTextEditorTextInsertAction.Value);
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