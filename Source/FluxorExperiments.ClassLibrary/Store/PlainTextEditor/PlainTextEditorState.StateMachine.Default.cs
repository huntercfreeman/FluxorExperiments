using FluxorExperiments.ClassLibrary.Keyboard;
using FluxorExperiments.ClassLibrary.KeyDownEvent;
using FluxorExperiments.ClassLibrary.PlainTextEditor;

namespace FluxorExperiments.ClassLibrary.Store.PlainTextEditor;

public partial record PlainTextEditorState
{
	private static partial class PlainTextEditorStateMachine
	{
		private static PlainTextEditorState GetNextStateFromDefault(PlainTextEditorState nextPlainTextEditorState,
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
						PerformDefaultPlainTextTokenBackspace(nextPlainTextEditorState, keyDownEventRecord);
						break;
				}
			}
			else
			{
				var nextDefaultToken = new DefaultPlainTextToken(keyDownEventRecord,
					(DefaultPlainTextToken)nextPlainTextEditorState.CurrentPlainTextToken);

				var nextRow = nextPlainTextEditorState.CurrentRow
					.ConstructDeepClone();

				nextRow = nextRow.WithReplace(nextDefaultToken.KeyRecord,
					nextDefaultToken);

				nextPlainTextEditorState.FeatureStateMap[nextRow.KeyRecord] = nextRow;
			}

			return nextPlainTextEditorState;
		}

		private static void PerformDefaultPlainTextTokenBackspace(PlainTextEditorState nextPlainTextEditorState,
			KeyDownEventRecord keyDownEventRecord)
		{
			int i = 1;
			bool Continue() => keyDownEventRecord.CtrlWasPressed || i-- > 0;

			DefaultPlainTextToken nextDefaultToken = 
				(DefaultPlainTextToken) nextPlainTextEditorState.CurrentPlainTextToken;
			
			while (nextDefaultToken.IndexInPlainText != -1 && Continue())
			{
				nextDefaultToken = new DefaultPlainTextToken(keyDownEventRecord,
					nextDefaultToken);
			}

			if (nextDefaultToken.IndexInPlainText == -1)
			{
				if (nextDefaultToken.AsPlainTextSpan.Length == 0)
				{
					PerformBackspace(nextPlainTextEditorState, keyDownEventRecord);		
				}
				else
				{
					SetPreviousTokenAsCurrentToken(nextPlainTextEditorState, keyDownEventRecord);
				}
			}

			var nextRow = nextPlainTextEditorState.CurrentRow
				.ConstructDeepClone();

			nextRow = nextRow.WithReplace(nextDefaultToken.KeyRecord,
				nextDefaultToken);

			nextPlainTextEditorState.FeatureStateMap[nextRow.KeyRecord] = nextRow;
		}
	}
}