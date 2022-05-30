using Fluxor;
using FluxorExperiments.ClassLibrary.Store.KeyDownEvent;

namespace FluxorExperiments.ClassLibrary.Store.PlainTextEditor;

public class PlainTextEditorReducer
{
	[ReducerMethod]
	public static PlainTextEditorState ReduceKeyDownEventAction(PlainTextEditorState previousPlainTextEditorState,
		KeyDownEventAction keyDownEventAction)
	{
		Console.WriteLine($"Key: {keyDownEventAction.OnKeyDownEventRecord.Key} was pressed.");

		return previousPlainTextEditorState.GetNextState(keyDownEventAction.OnKeyDownEventRecord);
	}
	
	[ReducerMethod]
	public static PlainTextEditorState ReducePlainTextEditorCharacterOnClickAction(PlainTextEditorState previousPlainTextEditorState,
		PlainTextEditorCharacterOnClickAction plainTextEditorCharacterOnClickAction)
	{
		return previousPlainTextEditorState.GetNextState(plainTextEditorCharacterOnClickAction.RowIndex,
			plainTextEditorCharacterOnClickAction.PlainTextTokenKeyIndex,
			plainTextEditorCharacterOnClickAction.CharacterIndex);
	}
}