using FluxorExperiments.ClassLibrary.Keyboard;
using FluxorExperiments.ClassLibrary.KeyDownEvent;
using FluxorExperiments.ClassLibrary.PlainTextEditor;

namespace FluxorExperiments.ClassLibrary.Store.PlainTextEditor;

public partial record PlainTextEditorState
{
	private static partial class PlainTextEditorStateMachine
	{
		private static PlainTextEditorState GetNextStateFromWhitespace(PlainTextEditorState nextPlainTextEditorState,
			KeyDownEventRecord keyDownEventRecord)
		{
			if (KeyboardFacts.IsWhitespaceKey(keyDownEventRecord))
			{
				HandleWhitespace(nextPlainTextEditorState, keyDownEventRecord);
			}
			else if (KeyboardFacts.IsMovementKey(keyDownEventRecord))
			{
				PerformMove(nextPlainTextEditorState, keyDownEventRecord);
			}
			else if (KeyboardFacts.IsMetaKey(keyDownEventRecord))
			{
				switch (keyDownEventRecord.Key)
				{
					case KeyboardFacts.MetaKeys.BACKSPACE:
						PerformBackspace(nextPlainTextEditorState, keyDownEventRecord);
						break;
				}
			}
			else
			{
				SetIndexInPlainTextOfCurrentToken(nextPlainTextEditorState, null);
				
				var defaultToken = new DefaultPlainTextToken(keyDownEventRecord);

				var nextRow = new PlainTextRow(nextPlainTextEditorState.CurrentRow);

				nextRow = nextRow.WithInsert(defaultToken.PlainTextTokenKey,
					defaultToken,
					nextPlainTextEditorState.CurrentPlainTextTokenKeyIndex + 1);

				nextPlainTextEditorState._plainTextRowMap[nextRow.PlainTextRowKey] = nextRow;

				nextPlainTextEditorState.CurrentPlainTextTokenKeyIndex += 1;
			}

			return nextPlainTextEditorState;
		}
	}
}