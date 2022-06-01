using FluxorExperiments.ClassLibrary.Keyboard;
using FluxorExperiments.ClassLibrary.KeyDownEvent;
using FluxorExperiments.ClassLibrary.PlainTextEditor;

namespace FluxorExperiments.ClassLibrary.Store.PlainTextEditor;

public partial record PlainTextEditorState
{
	private static partial class PlainTextEditorStateMachine
	{
		private static PlainTextEditorState GetNextStateFromStartOfRow(PlainTextEditorState nextPlainTextEditorState,
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

				var nextRow = nextPlainTextEditorState.CurrentRow.ConstructDeepClone();

				nextRow = nextRow.WithInsert(defaultToken.KeyRecord,
					defaultToken,
					nextPlainTextEditorState.CurrentPlainTextTokenKeyIndex + 1);

				nextPlainTextEditorState.FeatureStateMap[nextRow.KeyRecord] = nextRow;

				nextPlainTextEditorState.CurrentPlainTextTokenKeyIndex += 1;
			}

			return nextPlainTextEditorState;
		}
	}
}