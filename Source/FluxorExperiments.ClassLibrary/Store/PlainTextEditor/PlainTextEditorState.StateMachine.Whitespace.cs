﻿using FluxorExperiments.ClassLibrary.Keyboard;
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
			SetIndexInContentOfCurrentToken(nextPlainTextEditorState, null);

			if (KeyboardFacts.IsWhitespaceKey(keyDownEventRecord))
			{
				if (KeyboardFacts.WhitespaceKeys.ENTER_CODE == keyDownEventRecord.Code)
				{
					var row = new PlainTextRow();

					nextPlainTextEditorState._plainTextRowKeys
						.Insert(nextPlainTextEditorState.CurrentRowIndex + 1, row.PlainTextRowKey);

					nextPlainTextEditorState._plainTextRowMap.Add(row.PlainTextRowKey, row);

					nextPlainTextEditorState.CurrentRowIndex++;

					nextPlainTextEditorState.CurrentPlainTextTokenKeyIndex = 0;
				}
				else
				{
					var whitespaceToken = new WhitespacePlainTextToken(keyDownEventRecord);

					var nextRow = new PlainTextRow(nextPlainTextEditorState.CurrentRow);

					nextRow = nextRow.WithInsert(whitespaceToken.PlainTextTokenKey,
						whitespaceToken,
						nextPlainTextEditorState.CurrentPlainTextTokenKeyIndex + 1);

					nextPlainTextEditorState._plainTextRowMap[nextRow.PlainTextRowKey] = nextRow;

					nextPlainTextEditorState.CurrentPlainTextTokenKeyIndex += 1;
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

				nextPlainTextEditorState.CurrentPlainTextTokenKeyIndex += 1;
			}

			return nextPlainTextEditorState;
		}
	}
}