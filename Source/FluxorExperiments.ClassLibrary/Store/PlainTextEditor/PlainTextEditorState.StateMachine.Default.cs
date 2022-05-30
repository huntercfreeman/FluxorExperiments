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
				SetIndexInPlainTextOfCurrentToken(nextPlainTextEditorState, null);

				if (KeyboardFacts.WhitespaceKeys.ENTER_CODE == keyDownEventRecord.Code)
				{
					var row = new PlainTextRow();

					nextPlainTextEditorState._plainTextRowKeys
						.Insert(nextPlainTextEditorState.CurrentRowIndex + 1, row.PlainTextRowKey);

					nextPlainTextEditorState._plainTextRowMap.Add(row.PlainTextRowKey, row);

					nextPlainTextEditorState.CurrentPlainTextTokenKeyIndex = 0;
					nextPlainTextEditorState.CurrentRowIndex++;
				}
				else
				{
					var nextWhitespaceToken = new WhitespacePlainTextToken(keyDownEventRecord);

					var nextRow = new PlainTextRow(nextPlainTextEditorState.CurrentRow);

					nextRow = nextRow.WithInsert(nextWhitespaceToken.PlainTextTokenKey,
						nextWhitespaceToken,
						nextPlainTextEditorState.CurrentPlainTextTokenKeyIndex + 1);

					nextPlainTextEditorState._plainTextRowMap[nextRow.PlainTextRowKey] = nextRow;

					nextPlainTextEditorState.CurrentPlainTextTokenKeyIndex += 1;
				}
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

				var nextRow = new PlainTextRow(nextPlainTextEditorState.CurrentRow);

				nextRow = nextRow.WithReplace(nextDefaultToken.PlainTextTokenKey,
					nextDefaultToken);

				nextPlainTextEditorState._plainTextRowMap[nextRow.PlainTextRowKey] = nextRow;
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
				if (nextDefaultToken.ToPlainText.Length == 0)
				{
					PerformBackspace(nextPlainTextEditorState, keyDownEventRecord);		
				}
				else
				{
					SetPreviousTokenAsCurrentToken(nextPlainTextEditorState, keyDownEventRecord);
				}
			}

			var nextRow = new PlainTextRow(nextPlainTextEditorState.CurrentRow);

			nextRow = nextRow.WithReplace(nextDefaultToken.PlainTextTokenKey,
				nextDefaultToken);

			nextPlainTextEditorState._plainTextRowMap[nextRow.PlainTextRowKey] = nextRow;
		}
	}
}