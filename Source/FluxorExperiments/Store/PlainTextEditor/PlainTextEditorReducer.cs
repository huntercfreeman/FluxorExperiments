using Fluxor;

namespace FluxorExperiments.Store.PlainTextEditor;

public class PlainTextEditorReducer
{
	[ReducerMethod]
	public static PlainTextEditorState ReduceKeyDownEventAction(PlainTextEditorState previousPlainTextEditorState,
		KeyDownEventAction keyDownEventAction)
	{
		Console.WriteLine($"Key: {keyDownEventAction.OnKeyDownEventRecord.Key} was pressed.");

		return previousPlainTextEditorState.GetNextState(keyDownEventAction.OnKeyDownEventRecord);
	}
}