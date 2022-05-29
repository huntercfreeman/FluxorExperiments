using Fluxor;
using FluxorExperiments.Classes.Keyboard;
using FluxorExperiments.Classes.KeyDownEvent;
using FluxorExperiments.Classes.PlainTextEditor;

namespace FluxorExperiments.Store.PlainTextEditor;

public partial record PlainTextEditorState
{
	private static partial class PlainTextEditorStateMachine
	{
		public static PlainTextEditorState GetNextStateFromDefault(PlainTextEditorState nextPlainTextEditorState,
			KeyDownEventRecord keyDownEventRecord)
		{
			if (KeyboardFacts.IsWhitespaceKey(keyDownEventRecord))
			{
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
	}
}