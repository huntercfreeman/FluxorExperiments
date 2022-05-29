using FluxorExperiments.ClassLibrary.Keyboard;
using FluxorExperiments.ClassLibrary.KeyDownEvent;
using FluxorExperiments.ClassLibrary.PlainTextEditor;
using FluxorExperiments.ClassLibrary.Sequence;

namespace FluxorExperiments.ClassLibrary.Store.PlainTextEditor;

public partial record PlainTextEditorState
{
	private static partial class PlainTextEditorStateMachine
	{
		public static PlainTextEditorState GetNextState(PlainTextEditorState plainTextEditorState,
			KeyDownEventRecord keyDownEventRecord)
		{
			var nextPlainTextEditorState = new PlainTextEditorState(plainTextEditorState);

			return nextPlainTextEditorState.CurrentPlainTextToken.PlainTextTokenKind switch {
				PlainTextTokenKind.StartOfRow => GetNextStateFromStartOfRow(nextPlainTextEditorState,
					keyDownEventRecord),
				PlainTextTokenKind.Default => GetNextStateFromDefault(nextPlainTextEditorState, keyDownEventRecord),
				PlainTextTokenKind.Whitespace => GetNextStateFromWhitespace(nextPlainTextEditorState,
					keyDownEventRecord),
				_ => throw new ApplicationException($"The " +
				                                    $"{nameof(nextPlainTextEditorState.CurrentPlainTextToken.PlainTextTokenKind)} " +
				                                    $"of {nextPlainTextEditorState.CurrentPlainTextToken.PlainTextTokenKind} " +
				                                    $"is not currently supported.")
			};
		}

		private static void SetIndexInContentOfCurrentToken(PlainTextEditorState nextPlainTextEditorState,
			int? indexInContent)
		{
			var previousToken = nextPlainTextEditorState.CurrentPlainTextToken;

			var previousRow = nextPlainTextEditorState.CurrentRow;

			nextPlainTextEditorState._plainTextRowMap[previousRow.PlainTextRowKey] = previousRow
				.WithReplace(previousToken.PlainTextTokenKey, previousToken with {
					IndexInContent = indexInContent,
					SequenceKey = SequenceKey.NewSequenceKey()
				});
		}

		private static void PerformMove(PlainTextEditorState nextPlainTextEditorState,
			KeyDownEventRecord keyDownEventRecord)
		{
			switch (keyDownEventRecord.Key)
			{
				case KeyboardFacts.MovementKeys.ARROW_LEFT_KEY:
					MoveArrowLeft(nextPlainTextEditorState, keyDownEventRecord);
					break;
				case KeyboardFacts.MovementKeys.ARROW_DOWN_KEY:
					MoveArrowDown(nextPlainTextEditorState, keyDownEventRecord);
					break;
				case KeyboardFacts.MovementKeys.ARROW_UP_KEY:
					MoveArrowUp(nextPlainTextEditorState, keyDownEventRecord);
					break;
				case KeyboardFacts.MovementKeys.ARROW_RIGHT_KEY:
					MoveArrowRight(nextPlainTextEditorState, keyDownEventRecord);
					break;
				case KeyboardFacts.MovementKeys.HOME_KEY:
					MoveHome(nextPlainTextEditorState, keyDownEventRecord);
					break;
				case KeyboardFacts.MovementKeys.END_KEY:
					MoveEnd(nextPlainTextEditorState, keyDownEventRecord);
					break;
			}
		}

		private static void SetCurrentRowIsActiveRow(PlainTextEditorState nextPlainTextEditorState,
			KeyDownEventRecord keyDownEventRecord,
			bool isActiveRow)
		{
			 nextPlainTextEditorState._plainTextRowMap[nextPlainTextEditorState.CurrentRow.PlainTextRowKey] =
				  nextPlainTextEditorState.CurrentRow with 
				  {
					  IsActiveRow = isActiveRow,
					  SequenceKey = SequenceKey.NewSequenceKey()
				  };
		}
		
		private static void SetPreviousTokenAsCurrentToken(PlainTextEditorState nextPlainTextEditorState,
			KeyDownEventRecord keyDownEventRecord)
		{
			if (nextPlainTextEditorState.CurrentPlainTextTokenKeyIndex <= 0)
			{
				if (nextPlainTextEditorState.CurrentRowIndex > 0)
				{
					SetIndexInContentOfCurrentToken(nextPlainTextEditorState, null);
					SetCurrentRowIsActiveRow(nextPlainTextEditorState, keyDownEventRecord, false);
					
					nextPlainTextEditorState.CurrentRowIndex--;
					nextPlainTextEditorState.CurrentPlainTextTokenKeyIndex = 
						nextPlainTextEditorState.CurrentRow.TokenCount - 1;
					
					SetIndexInContentOfCurrentToken(nextPlainTextEditorState, 0);

					SetCurrentRowIsActiveRow(nextPlainTextEditorState, keyDownEventRecord, true);
				}
			}
			else
			{
				 SetIndexInContentOfCurrentToken(nextPlainTextEditorState, null);

				 nextPlainTextEditorState.CurrentPlainTextTokenKeyIndex--;
				 
				 SetIndexInContentOfCurrentToken(nextPlainTextEditorState, 
					 nextPlainTextEditorState.CurrentPlainTextToken.ToPlainText.Length - 1);
			}
		}

		private static void SetNextTokenAsCurrentToken(PlainTextEditorState nextPlainTextEditorState,
			KeyDownEventRecord keyDownEventRecord)
		{
			if (nextPlainTextEditorState.CurrentPlainTextTokenKeyIndex >= 
			    nextPlainTextEditorState.CurrentRow.TokenCount - 1)
			{
				if (nextPlainTextEditorState.CurrentRowIndex < 
				    nextPlainTextEditorState.RowCount - 1)
				{
					SetIndexInContentOfCurrentToken(nextPlainTextEditorState, null);
					SetCurrentRowIsActiveRow(nextPlainTextEditorState, keyDownEventRecord, false);
					
					nextPlainTextEditorState.CurrentPlainTextTokenKeyIndex = 0;
					nextPlainTextEditorState.CurrentRowIndex++;
					
					SetIndexInContentOfCurrentToken(nextPlainTextEditorState, 0);
					
					SetCurrentRowIsActiveRow(nextPlainTextEditorState, keyDownEventRecord, true);
				}
			}
			else
			{
				 SetIndexInContentOfCurrentToken(nextPlainTextEditorState, null);

				 nextPlainTextEditorState.CurrentPlainTextTokenKeyIndex++;
				 
				 SetIndexInContentOfCurrentToken(nextPlainTextEditorState, 0);
			}
		}
		
		private static void MoveArrowLeft(PlainTextEditorState nextPlainTextEditorState,
			KeyDownEventRecord keyDownEventRecord)
		{
			if (nextPlainTextEditorState.CurrentPlainTextToken.IndexInContent == 0)
			{
				SetIndexInContentOfCurrentToken(nextPlainTextEditorState, null);

				SetPreviousTokenAsCurrentToken(nextPlainTextEditorState, keyDownEventRecord);
			}
			else
			{
				 SetIndexInContentOfCurrentToken(nextPlainTextEditorState, 
					 nextPlainTextEditorState.CurrentPlainTextToken.IndexInContent - 1);
			}
		}

		private static void MoveArrowDown(PlainTextEditorState nextPlainTextEditorState,
			KeyDownEventRecord keyDownEventRecord)
		{
			throw new NotImplementedException();
		}

		private static void MoveArrowUp(PlainTextEditorState nextPlainTextEditorState,
			KeyDownEventRecord keyDownEventRecord)
		{
			throw new NotImplementedException();
		}

		private static void MoveArrowRight(PlainTextEditorState nextPlainTextEditorState,
			KeyDownEventRecord keyDownEventRecord)
		{
			throw new NotImplementedException();
		}

		private static void MoveHome(PlainTextEditorState nextPlainTextEditorState,
			KeyDownEventRecord keyDownEventRecord)
		{
			throw new NotImplementedException();
		}

		private static void MoveEnd(PlainTextEditorState nextPlainTextEditorState,
			KeyDownEventRecord keyDownEventRecord)
		{
			throw new NotImplementedException();
		}
	}
}