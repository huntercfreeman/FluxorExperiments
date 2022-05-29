using Fluxor;
using FluxorExperiments.Classes.Keyboard;
using FluxorExperiments.Classes.KeyDownEvent;
using FluxorExperiments.Classes.PlainTextEditor;

namespace FluxorExperiments.Store.PlainTextEditor;

public partial record PlainTextEditorState
{
	private static partial class PlainTextEditorStateMachine
	{
		public static PlainTextEditorState GetNextStateFromDefault(PlainTextEditorState nextPlainTextEditorState, KeyDownEventRecord keyDownEventRecord)
		{
			if (KeyboardFacts.IsWhitespaceKey(keyDownEventRecord))
			{
				var nextWhitespaceToken = new WhitespacePlainTextToken(keyDownEventRecord);

				var nextRow = new PlainTextRow(nextPlainTextEditorState.CurrentRow);

				nextRow = nextRow.WithInsert(nextWhitespaceToken.PlainTextTokenKey, 
					nextWhitespaceToken, 
					nextPlainTextEditorState.CurrentPlainTextTokenKeyIndex + 1);

				nextPlainTextEditorState._plainTextRowMap[nextRow.PlainTextRowKey] = nextRow;
			}
			else
			{
				var nextDefaultToken = new DefaultPlainTextToken(keyDownEventRecord, 
					(DefaultPlainTextToken) nextPlainTextEditorState.CurrentPlainTextToken);
				
				var nextRow = new PlainTextRow(nextPlainTextEditorState.CurrentRow);

				nextRow = nextRow.WithReplace(nextDefaultToken.PlainTextTokenKey, 
					nextDefaultToken);

				nextPlainTextEditorState._plainTextRowMap[nextRow.PlainTextRowKey] = nextRow;
			}

			return nextPlainTextEditorState;
		}
	}
}