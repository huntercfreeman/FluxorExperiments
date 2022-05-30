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
					IndexInPlainText = indexInContent,
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
				nextPlainTextEditorState.CurrentRow with {
					IsActiveRow = isActiveRow,
					SequenceKey = SequenceKey.NewSequenceKey()
				};
		}

		private static bool SetPreviousTokenAsCurrentToken(PlainTextEditorState nextPlainTextEditorState,
			KeyDownEventRecord keyDownEventRecord)
		{
			var previousWasSetAsCurrent = false;

			if (nextPlainTextEditorState.CurrentPlainTextTokenKeyIndex <= 0)
			{
				if (nextPlainTextEditorState.CurrentRowIndex > 0)
				{
					SetIndexInContentOfCurrentToken(nextPlainTextEditorState, null);
					SetCurrentRowIsActiveRow(nextPlainTextEditorState, keyDownEventRecord, false);

					nextPlainTextEditorState.CurrentRowIndex--;
					nextPlainTextEditorState.CurrentPlainTextTokenKeyIndex =
						nextPlainTextEditorState.CurrentRow.TokenCount - 1;

					SetIndexInContentOfCurrentToken(nextPlainTextEditorState,
						nextPlainTextEditorState.CurrentPlainTextToken.ToPlainText.Length - 1);

					SetCurrentRowIsActiveRow(nextPlainTextEditorState, keyDownEventRecord, true);

					previousWasSetAsCurrent = true;
				}
			}
			else
			{
				SetIndexInContentOfCurrentToken(nextPlainTextEditorState, null);

				nextPlainTextEditorState.CurrentPlainTextTokenKeyIndex--;

				SetIndexInContentOfCurrentToken(nextPlainTextEditorState,
					nextPlainTextEditorState.CurrentPlainTextToken.ToPlainText.Length - 1);

				previousWasSetAsCurrent = true;
			}

			return previousWasSetAsCurrent;
		}

		private static bool SetNextTokenAsCurrentToken(PlainTextEditorState nextPlainTextEditorState,
			KeyDownEventRecord keyDownEventRecord)
		{
			var nextWasSetAsCurrent = false;

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

					nextWasSetAsCurrent = true;
				}
			}
			else
			{
				SetIndexInContentOfCurrentToken(nextPlainTextEditorState, null);

				nextPlainTextEditorState.CurrentPlainTextTokenKeyIndex++;

				SetIndexInContentOfCurrentToken(nextPlainTextEditorState, 0);

				nextWasSetAsCurrent = true;
			}

			return nextWasSetAsCurrent;
		}

		private static void MoveArrowLeft(PlainTextEditorState nextPlainTextEditorState,
			KeyDownEventRecord keyDownEventRecord)
		{
			if (nextPlainTextEditorState.CurrentPlainTextToken.IndexInPlainText == 0)
			{
				SetIndexInContentOfCurrentToken(nextPlainTextEditorState, null);

				var previousWasSetAsCurrent =
					SetPreviousTokenAsCurrentToken(nextPlainTextEditorState, keyDownEventRecord);

				if (!previousWasSetAsCurrent)
					SetIndexInContentOfCurrentToken(nextPlainTextEditorState, 0);
			}
			else
			{
				SetIndexInContentOfCurrentToken(nextPlainTextEditorState,
					nextPlainTextEditorState.CurrentPlainTextToken.IndexInPlainText - 1);
			}
		}

		private static void MoveArrowDown(PlainTextEditorState nextPlainTextEditorState,
			KeyDownEventRecord keyDownEventRecord)
		{
			if (nextPlainTextEditorState.CurrentRowIndex >= nextPlainTextEditorState.RowCount - 1)
				return;
			
			var inclusiveStartingColumnIndexOfCurrentToken =
				CalculateCurrentTokenColumnIndexRespectiveToRow(nextPlainTextEditorState);

			var currentColumnIndexWithIndexInPlainTextAccountedFor = inclusiveStartingColumnIndexOfCurrentToken +
			                                                         nextPlainTextEditorState.CurrentPlainTextToken
				                                                         .IndexInPlainText!.Value;

			var tokenInRowBelowMetaData = CalculateTokenAtColumnIndexRespectiveToRow(
				nextPlainTextEditorState,
				nextPlainTextEditorState.GetRowAtIndex(nextPlainTextEditorState.CurrentRowIndex + 1),
				currentColumnIndexWithIndexInPlainTextAccountedFor);

			while (nextPlainTextEditorState.CurrentPlainTextToken.PlainTextTokenKey != 
			       tokenInRowBelowMetaData.PlainTextToken.PlainTextTokenKey)
			{
				MoveArrowRight(nextPlainTextEditorState, new KeyDownEventRecord(KeyboardFacts.MovementKeys.ARROW_RIGHT_KEY,
					KeyboardFacts.MovementKeys.ARROW_RIGHT_KEY,
					false,
					false,
					false));
			}

			if (currentColumnIndexWithIndexInPlainTextAccountedFor < 
			    tokenInRowBelowMetaData.PositionSpanRelativeToRowRecord!.ExclusiveEndingColumnIndex)
			{
				 SetIndexInContentOfCurrentToken(nextPlainTextEditorState, 
					 currentColumnIndexWithIndexInPlainTextAccountedFor - 
						  tokenInRowBelowMetaData.PositionSpanRelativeToRowRecord!.InclusiveStartingColumnIndex);
			}
			else
			{
				SetIndexInContentOfCurrentToken(nextPlainTextEditorState,
					nextPlainTextEditorState.CurrentPlainTextToken.ToPlainText.Length - 1);
			}
		}

		private static void MoveArrowUp(PlainTextEditorState nextPlainTextEditorState,
			KeyDownEventRecord keyDownEventRecord)
		{
			if (nextPlainTextEditorState.CurrentRowIndex <= 0)
				return;
			
			var inclusiveStartingColumnIndexOfCurrentToken =
				CalculateCurrentTokenColumnIndexRespectiveToRow(nextPlainTextEditorState);

			var currentColumnIndexWithIndexInPlainTextAccountedFor = inclusiveStartingColumnIndexOfCurrentToken +
			                                                         nextPlainTextEditorState.CurrentPlainTextToken
				                                                         .IndexInPlainText!.Value;

			var tokenInRowAboveMetaData = CalculateTokenAtColumnIndexRespectiveToRow(
				nextPlainTextEditorState,
				nextPlainTextEditorState.GetRowAtIndex(nextPlainTextEditorState.CurrentRowIndex - 1),
				currentColumnIndexWithIndexInPlainTextAccountedFor);

			while (nextPlainTextEditorState.CurrentPlainTextToken.PlainTextTokenKey != 
			       tokenInRowAboveMetaData.PlainTextToken.PlainTextTokenKey)
			{
				MoveArrowLeft(nextPlainTextEditorState, new KeyDownEventRecord(KeyboardFacts.MovementKeys.ARROW_LEFT_KEY,
					KeyboardFacts.MovementKeys.ARROW_LEFT_KEY,
					false,
					false,
					false));
			}

			if (currentColumnIndexWithIndexInPlainTextAccountedFor < 
			    tokenInRowAboveMetaData.PositionSpanRelativeToRowRecord!.ExclusiveEndingColumnIndex)
			{
				 SetIndexInContentOfCurrentToken(nextPlainTextEditorState, 
					 currentColumnIndexWithIndexInPlainTextAccountedFor - 
						  tokenInRowAboveMetaData.PositionSpanRelativeToRowRecord!.InclusiveStartingColumnIndex);
			}
			else
			{
				SetIndexInContentOfCurrentToken(nextPlainTextEditorState,
					nextPlainTextEditorState.CurrentPlainTextToken.ToPlainText.Length - 1);
			}
		}

		private static void MoveArrowRight(PlainTextEditorState nextPlainTextEditorState,
			KeyDownEventRecord keyDownEventRecord)
		{
			if (nextPlainTextEditorState.CurrentPlainTextToken.IndexInPlainText ==
			    nextPlainTextEditorState.CurrentPlainTextToken.ToPlainText.Length - 1)
			{
				SetIndexInContentOfCurrentToken(nextPlainTextEditorState, null);

				var nextWasSetAsCurrent = SetNextTokenAsCurrentToken(nextPlainTextEditorState, keyDownEventRecord);

				if (!nextWasSetAsCurrent)
					SetIndexInContentOfCurrentToken(nextPlainTextEditorState,
						nextPlainTextEditorState.CurrentPlainTextToken.ToPlainText.Length - 1);
			}
			else
			{
				SetIndexInContentOfCurrentToken(nextPlainTextEditorState,
					nextPlainTextEditorState.CurrentPlainTextToken.IndexInPlainText + 1);
			}
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

		/// <summary>
		/// Returns the inclusive starting column index
		/// </summary>
		/// <param name="nextPlainTextEditorState"></param>
		/// <returns></returns>
		private static int CalculateCurrentTokenColumnIndexRespectiveToRow(
			PlainTextEditorState nextPlainTextEditorState)
		{
			var rollingCount = 0;

			foreach (var tokenKey in nextPlainTextEditorState.CurrentRow.PlainTextTokenKeys)
			{
				var token = nextPlainTextEditorState.CurrentRow.LookupPlainTextToken(tokenKey);

				if (tokenKey == nextPlainTextEditorState.CurrentPlainTextToken.PlainTextTokenKey)
				{
					return rollingCount;
				}
				else
				{
					rollingCount += token.ToPlainText.Length;
				}
			}

			return 0;
		}

		private static PlainTextTokenMetaData CalculateTokenAtColumnIndexRespectiveToRow(
			PlainTextEditorState nextPlainTextEditorState,
			PlainTextRow row,
			int columnIndex)
		{
			var rollingCount = 0;
			PlainTextTokenBase? token = row.GetPlainTextTokenFromIndex(0);

			foreach (var tokenKey in row.PlainTextTokenKeys)
			{
				token = row.LookupPlainTextToken(tokenKey);

				rollingCount += token.ToPlainText.Length;

				if (rollingCount >= columnIndex)
					break;
			}

			var positionRelativeToRow = new PositionSpanRelativeToRowRecord(
				rollingCount - token.ToPlainText.Length,
				rollingCount);

			return new PlainTextTokenMetaData(token, positionRelativeToRow, null);
		}
	}
}