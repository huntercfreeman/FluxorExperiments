using Fluxor;
using FluxorExperiments.Classes.Keyboard;
using FluxorExperiments.Classes.KeyDownEvent;
using FluxorExperiments.Classes.PlainTextEditor;

namespace FluxorExperiments.Store.PlainTextEditor;

public partial record PlainTextEditorState
{
	private static partial class PlainTextEditorStateMachine
	{
		public static PlainTextEditorState GetNextStateFromWhitespace(PlainTextEditorState nextPlainTextEditorState, KeyDownEventRecord keyDownEventRecord)
		{
			if (KeyboardFacts.IsWhitespaceKey(keyDownEventRecord))
			{
				if (KeyboardFacts.WhitespaceKeys.ENTER_CODE == keyDownEventRecord.Code)
				{
					var row = new PlainTextRow();

					nextPlainTextEditorState._plainTextRowKeys
						.Insert(nextPlainTextEditorState.CurrentRowIndex + 1, row.PlainTextRowKey);

					nextPlainTextEditorState._plainTextRowMap.Add(row.PlainTextRowKey, row);
				}
				else
				{
					 var whitespaceToken = new WhitespacePlainTextToken(keyDownEventRecord);

					 var nextRow = new PlainTextRow(nextPlainTextEditorState.CurrentRow);

					 nextRow = nextRow.WithInsert(whitespaceToken.PlainTextTokenKey, 
						 whitespaceToken, 
						 nextPlainTextEditorState.CurrentPlainTextTokenKeyIndex + 1);

					 nextPlainTextEditorState._plainTextRowMap[nextRow.PlainTextRowKey] = nextRow;
				}
			}
			else
			{
				var defaultToken = new DefaultPlainTextToken(keyDownEventRecord);

				var nextRow = new PlainTextRow(nextPlainTextEditorState.CurrentRow);

				nextRow = nextRow.WithInsert(defaultToken.PlainTextTokenKey, 
					defaultToken, 
					nextPlainTextEditorState.CurrentPlainTextTokenKeyIndex + 1);

				nextPlainTextEditorState._plainTextRowMap[nextRow.PlainTextRowKey] = nextRow;
			}

			return nextPlainTextEditorState;
		}
	}
}