using FluxorExperiments.ClassLibrary.Clipboard;
using FluxorExperiments.ClassLibrary.FeatureStateContainer;
using FluxorExperiments.ClassLibrary.Keyboard;
using FluxorExperiments.ClassLibrary.KeyDownEvent;
using FluxorExperiments.ClassLibrary.PlainTextEditor;

namespace FluxorExperiments.ClassLibrary.Store.PlainTextEditor;

public partial record PlainTextEditorState
{
	private static partial class PlainTextEditorStateMachine
	{
		public static PlainTextEditorState GetNextState(PlainTextEditorState plainTextEditorState,
			KeyDownEventRecord keyDownEventRecord)
		{
			var nextPlainTextEditorState = new PlainTextEditorState(plainTextEditorState);

			if (keyDownEventRecord.Key == "c" && keyDownEventRecord.CtrlWasPressed)
			{
				return nextPlainTextEditorState;
			}

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

		public static PlainTextEditorState GetNextState(PlainTextEditorState plainTextEditorState,
			int rowIndex,
			int plainTextTokenKeyIndex,
			int characterIndex)
		{
			var nextPlainTextEditorState = new PlainTextEditorState(plainTextEditorState);

			SetCurrentRowIsActiveRow(nextPlainTextEditorState, null, false);
			SetIndexInPlainTextOfCurrentToken(nextPlainTextEditorState, null);

			nextPlainTextEditorState.CurrentRowIndex = rowIndex;
			nextPlainTextEditorState.CurrentPlainTextTokenKeyIndex = plainTextTokenKeyIndex;

			SetCurrentRowIsActiveRow(nextPlainTextEditorState, null, true);
			SetIndexInPlainTextOfCurrentToken(nextPlainTextEditorState, characterIndex);

			return nextPlainTextEditorState;
		}
		
		public static PlainTextEditorState GetNextState(PlainTextEditorState plainTextEditorState, 
			string bulkStringInsertionValue)
		{
			var nextPlainTextEditorState = new PlainTextEditorState(plainTextEditorState);
			
			foreach (var character in bulkStringInsertionValue)
			{
				var code = character switch {
					'\n' => KeyboardFacts.WhitespaceKeys.ENTER_CODE,
					'\t' => KeyboardFacts.WhitespaceKeys.TAB_CODE,
					' ' => KeyboardFacts.WhitespaceKeys.SPACE_CODE,
					_ => character.ToString()
				};
				
				var keyDownEventRecord = new KeyDownEventRecord(character.ToString(), 
					code,
					false,
					false,
					false);
				
				nextPlainTextEditorState = nextPlainTextEditorState.CurrentPlainTextToken.PlainTextTokenKind switch {
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

			return nextPlainTextEditorState;
		}

		private static void SetIndexInPlainTextOfCurrentToken(PlainTextEditorState nextPlainTextEditorState,
			int? indexInContent)
		{
			var previousToken = nextPlainTextEditorState.CurrentPlainTextToken;

			var previousRow = nextPlainTextEditorState.CurrentRow;

			nextPlainTextEditorState.FeatureStateMap[previousRow.KeyRecord] = previousRow
				.WithReplace(previousToken.KeyRecord, previousToken with {
					IndexInPlainText = indexInContent,
					SequenceKeyRecord = new SequenceKeyRecord()
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
			nextPlainTextEditorState.FeatureStateMap[nextPlainTextEditorState.CurrentRow.KeyRecord] =
				nextPlainTextEditorState.CurrentRow with {
					IsActiveRow = isActiveRow,
					SequenceKeyRecord = new SequenceKeyRecord()
				};
		}

		private static bool SetPreviousTokenAsCurrentToken(PlainTextEditorState nextPlainTextEditorState,
			KeyDownEventRecord keyDownEventRecord,
			bool setCurrentTokenIndexInPlainTextToNullBeforeOperation = true)
		{
			var previousWasSetAsCurrent = false;

			if (nextPlainTextEditorState.CurrentPlainTextTokenKeyIndex <= 0)
			{
				if (nextPlainTextEditorState.CurrentRowIndex > 0)
				{
					if (setCurrentTokenIndexInPlainTextToNullBeforeOperation)
						SetIndexInPlainTextOfCurrentToken(nextPlainTextEditorState, null);

					SetCurrentRowIsActiveRow(nextPlainTextEditorState, keyDownEventRecord, false);

					nextPlainTextEditorState.CurrentRowIndex--;
					nextPlainTextEditorState.CurrentPlainTextTokenKeyIndex =
						nextPlainTextEditorState.CurrentRow.Count - 1;

					SetIndexInPlainTextOfCurrentToken(nextPlainTextEditorState,
						nextPlainTextEditorState.CurrentPlainTextToken.ToPlainText.Length - 1);

					SetCurrentRowIsActiveRow(nextPlainTextEditorState, keyDownEventRecord, true);

					previousWasSetAsCurrent = true;
				}
			}
			else
			{
				if (setCurrentTokenIndexInPlainTextToNullBeforeOperation)
					SetIndexInPlainTextOfCurrentToken(nextPlainTextEditorState, null);

				nextPlainTextEditorState.CurrentPlainTextTokenKeyIndex--;

				SetIndexInPlainTextOfCurrentToken(nextPlainTextEditorState,
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
			    nextPlainTextEditorState.CurrentRow.Count - 1)
			{
				if (nextPlainTextEditorState.CurrentRowIndex <
				    nextPlainTextEditorState.Count - 1)
				{
					SetIndexInPlainTextOfCurrentToken(nextPlainTextEditorState, null);
					SetCurrentRowIsActiveRow(nextPlainTextEditorState, keyDownEventRecord, false);

					nextPlainTextEditorState.CurrentPlainTextTokenKeyIndex = 0;
					nextPlainTextEditorState.CurrentRowIndex++;

					SetIndexInPlainTextOfCurrentToken(nextPlainTextEditorState, 0);

					SetCurrentRowIsActiveRow(nextPlainTextEditorState, keyDownEventRecord, true);

					nextWasSetAsCurrent = true;
				}
			}
			else
			{
				SetIndexInPlainTextOfCurrentToken(nextPlainTextEditorState, null);

				nextPlainTextEditorState.CurrentPlainTextTokenKeyIndex++;

				SetIndexInPlainTextOfCurrentToken(nextPlainTextEditorState, 0);

				nextWasSetAsCurrent = true;
			}

			return nextWasSetAsCurrent;
		}

		private static void MoveArrowLeft(PlainTextEditorState nextPlainTextEditorState,
			KeyDownEventRecord keyDownEventRecord)
		{
			PlainTextTokenMetaData? temporaryTokenMetaDataForTextSelection = null;
			int? temporaryTokenIndexInPlainTextForTextSelection = null;

			if (!keyDownEventRecord.ShiftWasPressed)
			{
				nextPlainTextEditorState.SelectionSpanRecord = null;
			}
			else
			{
				temporaryTokenMetaDataForTextSelection =
					CalculateCurrentTokenColumnIndexRespectiveToDocument(nextPlainTextEditorState);

				temporaryTokenIndexInPlainTextForTextSelection =
					nextPlainTextEditorState.CurrentPlainTextToken.IndexInPlainText!.Value;
			}

			if (nextPlainTextEditorState.CurrentPlainTextToken.IndexInPlainText == 0)
			{
				SetIndexInPlainTextOfCurrentToken(nextPlainTextEditorState, null);

				var previousWasSetAsCurrent =
					SetPreviousTokenAsCurrentToken(nextPlainTextEditorState, keyDownEventRecord);

				if (!previousWasSetAsCurrent)
				{
					SetIndexInPlainTextOfCurrentToken(nextPlainTextEditorState, 0);
				}
				else if (temporaryTokenMetaDataForTextSelection is not null)
				{
					PerformTextSelection(nextPlainTextEditorState,
						temporaryTokenMetaDataForTextSelection
							.PositionSpanRelativeToDocumentRecord!
							.InclusiveStartingDocumentIndex + temporaryTokenIndexInPlainTextForTextSelection!.Value,
						-1,
						SelectionDirectionBinding.Left);
				}
			}
			else
			{
				if (keyDownEventRecord.CtrlWasPressed)
				{
					var arrowLeftMovementsNeeded = nextPlainTextEditorState
						.CurrentPlainTextToken
						.IndexInPlainText + 1;

					for (int i = 0; i < arrowLeftMovementsNeeded; i++)
					{
						MoveArrowLeft(nextPlainTextEditorState,
							KeyDownEventRecord.CloneWithoutCtrlModifier(keyDownEventRecord));
					}
				}
				else
				{
					SetIndexInPlainTextOfCurrentToken(nextPlainTextEditorState,
						nextPlainTextEditorState.CurrentPlainTextToken.IndexInPlainText - 1);

					if (temporaryTokenMetaDataForTextSelection is not null)
					{
						PerformTextSelection(nextPlainTextEditorState,
							temporaryTokenMetaDataForTextSelection
								.PositionSpanRelativeToDocumentRecord!
								.InclusiveStartingDocumentIndex + temporaryTokenIndexInPlainTextForTextSelection!.Value,
							-1,
							SelectionDirectionBinding.Left);
					}
				}
			}
		}

		private static void MoveArrowDown(PlainTextEditorState nextPlainTextEditorState,
			KeyDownEventRecord keyDownEventRecord)
		{
			if (nextPlainTextEditorState.CurrentRowIndex >= nextPlainTextEditorState.Count - 1)
				return;

			var inclusiveStartingColumnIndexOfCurrentToken =
				CalculateCurrentTokenColumnIndexRespectiveToRow(nextPlainTextEditorState);

			var currentColumnIndexWithIndexInPlainTextAccountedFor = inclusiveStartingColumnIndexOfCurrentToken +
			                                                         nextPlainTextEditorState.CurrentPlainTextToken
				                                                         .IndexInPlainText!.Value;

			var tokenInRowBelowMetaData = CalculateTokenAtColumnIndexRespectiveToRow(
				nextPlainTextEditorState,
				nextPlainTextEditorState[nextPlainTextEditorState.CurrentRowIndex + 1],
				currentColumnIndexWithIndexInPlainTextAccountedFor);

			while (nextPlainTextEditorState.CurrentPlainTextToken.KeyRecord !=
			       tokenInRowBelowMetaData.PlainTextToken.KeyRecord)
			{
				MoveArrowRight(nextPlainTextEditorState, new KeyDownEventRecord(
					KeyboardFacts.MovementKeys.ARROW_RIGHT_KEY,
					KeyboardFacts.MovementKeys.ARROW_RIGHT_KEY,
					false,
					keyDownEventRecord.ShiftWasPressed,
					false));
			}

			if (currentColumnIndexWithIndexInPlainTextAccountedFor <
			    tokenInRowBelowMetaData.PositionSpanRelativeToRowRecord!.ExclusiveEndingColumnIndex)
			{
				SetIndexInPlainTextOfCurrentToken(nextPlainTextEditorState,
					currentColumnIndexWithIndexInPlainTextAccountedFor -
					tokenInRowBelowMetaData.PositionSpanRelativeToRowRecord!.InclusiveStartingColumnIndex);
			}
			else
			{
				SetIndexInPlainTextOfCurrentToken(nextPlainTextEditorState,
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
				nextPlainTextEditorState[nextPlainTextEditorState.CurrentRowIndex - 1],
				currentColumnIndexWithIndexInPlainTextAccountedFor);

			while (nextPlainTextEditorState.CurrentPlainTextToken.KeyRecord !=
			       tokenInRowAboveMetaData.PlainTextToken.KeyRecord)
			{
				MoveArrowLeft(nextPlainTextEditorState, new KeyDownEventRecord(
					KeyboardFacts.MovementKeys.ARROW_LEFT_KEY,
					KeyboardFacts.MovementKeys.ARROW_LEFT_KEY,
					false,
					keyDownEventRecord.ShiftWasPressed,
					false));
			}

			if (currentColumnIndexWithIndexInPlainTextAccountedFor <
			    tokenInRowAboveMetaData.PositionSpanRelativeToRowRecord!.ExclusiveEndingColumnIndex)
			{
				SetIndexInPlainTextOfCurrentToken(nextPlainTextEditorState,
					currentColumnIndexWithIndexInPlainTextAccountedFor -
					tokenInRowAboveMetaData.PositionSpanRelativeToRowRecord!.InclusiveStartingColumnIndex);
			}
			else
			{
				SetIndexInPlainTextOfCurrentToken(nextPlainTextEditorState,
					nextPlainTextEditorState.CurrentPlainTextToken.ToPlainText.Length - 1);
			}
		}

		private static void MoveArrowRight(PlainTextEditorState nextPlainTextEditorState,
			KeyDownEventRecord keyDownEventRecord)
		{
			PlainTextTokenMetaData? temporaryTokenMetaDataForTextSelection = null;
			int? temporaryTokenIndexInPlainTextForTextSelection = null;

			if (!keyDownEventRecord.ShiftWasPressed)
			{
				nextPlainTextEditorState.SelectionSpanRecord = null;
			}
			else
			{
				temporaryTokenMetaDataForTextSelection =
					CalculateCurrentTokenColumnIndexRespectiveToDocument(nextPlainTextEditorState);

				temporaryTokenIndexInPlainTextForTextSelection =
					nextPlainTextEditorState.CurrentPlainTextToken.IndexInPlainText!.Value;
			}

			if (nextPlainTextEditorState.CurrentPlainTextToken.IndexInPlainText ==
			    nextPlainTextEditorState.CurrentPlainTextToken.ToPlainText.Length - 1)
			{
				var passOnArrowRightEvent = nextPlainTextEditorState.CurrentPlainTextToken.IndexInPlainText ==
				                            nextPlainTextEditorState.CurrentPlainTextToken.ToPlainText.Length - 1;

				SetIndexInPlainTextOfCurrentToken(nextPlainTextEditorState, null);

				var nextWasSetAsCurrent = SetNextTokenAsCurrentToken(nextPlainTextEditorState, keyDownEventRecord);

				if (!nextWasSetAsCurrent)
				{
					SetIndexInPlainTextOfCurrentToken(nextPlainTextEditorState,
						nextPlainTextEditorState.CurrentPlainTextToken.ToPlainText.Length - 1);
				}
				else
				{
					if (temporaryTokenMetaDataForTextSelection is not null)
					{
						PerformTextSelection(nextPlainTextEditorState,
							temporaryTokenMetaDataForTextSelection
								.PositionSpanRelativeToDocumentRecord!
								.InclusiveStartingDocumentIndex +
							temporaryTokenIndexInPlainTextForTextSelection!.Value,
							+1,
							SelectionDirectionBinding.Right);
					}

					if (passOnArrowRightEvent &&
					    keyDownEventRecord.CtrlWasPressed &&
					    nextPlainTextEditorState.CurrentPlainTextToken.ToPlainText.Length > 1)
					{
						MoveArrowRight(nextPlainTextEditorState, new KeyDownEventRecord(keyDownEventRecord.Key,
							keyDownEventRecord.Code,
							true,
							keyDownEventRecord.ShiftWasPressed,
							false));
					}
				}
			}
			else
			{
				if (keyDownEventRecord.CtrlWasPressed)
				{
					var arrowRightMovementsNeeded = nextPlainTextEditorState
							.CurrentPlainTextToken
							.ToPlainText.Length - 1
							                    - nextPlainTextEditorState
								                    .CurrentPlainTextToken
								                    .IndexInPlainText;

					for (int i = 0; i < arrowRightMovementsNeeded; i++)
					{
						MoveArrowRight(nextPlainTextEditorState,
							KeyDownEventRecord.CloneWithoutCtrlModifier(keyDownEventRecord));
					}
				}
				else
				{
					SetIndexInPlainTextOfCurrentToken(nextPlainTextEditorState,
						nextPlainTextEditorState.CurrentPlainTextToken.IndexInPlainText + 1);

					if (temporaryTokenMetaDataForTextSelection is not null)
					{
						PerformTextSelection(nextPlainTextEditorState,
							temporaryTokenMetaDataForTextSelection
								.PositionSpanRelativeToDocumentRecord!
								.InclusiveStartingDocumentIndex +
							temporaryTokenIndexInPlainTextForTextSelection!.Value + 1,
							+1,
							SelectionDirectionBinding.Right);
					}
				}
			}
		}

		private static void MoveHome(PlainTextEditorState nextPlainTextEditorState,
			KeyDownEventRecord keyDownEventRecord)
		{
			PlainTextTokenBase goalToken = keyDownEventRecord.CtrlWasPressed
				? nextPlainTextEditorState[0][0]
				: nextPlainTextEditorState.CurrentRow[0];

			while (nextPlainTextEditorState.CurrentPlainTextToken.KeyRecord !=
			       goalToken.KeyRecord)
			{
				MoveArrowLeft(nextPlainTextEditorState, new KeyDownEventRecord(
					KeyboardFacts.MovementKeys.ARROW_LEFT_KEY,
					KeyboardFacts.MovementKeys.ARROW_LEFT_KEY,
					false,
					keyDownEventRecord.ShiftWasPressed,
					false));
			}

			while (nextPlainTextEditorState.CurrentPlainTextToken.IndexInPlainText != 0)
			{
				MoveArrowLeft(nextPlainTextEditorState, new KeyDownEventRecord(
					KeyboardFacts.MovementKeys.ARROW_LEFT_KEY,
					KeyboardFacts.MovementKeys.ARROW_LEFT_KEY,
					false,
					keyDownEventRecord.ShiftWasPressed,
					false));
			}
		}

		private static void MoveEnd(PlainTextEditorState nextPlainTextEditorState,
			KeyDownEventRecord keyDownEventRecord)
		{
			var finalRow = nextPlainTextEditorState[nextPlainTextEditorState.Count - 1];

			PlainTextTokenBase goalToken = keyDownEventRecord.CtrlWasPressed
				? finalRow[^1]
				: nextPlainTextEditorState.CurrentRow[^1];

			while (nextPlainTextEditorState.CurrentPlainTextToken.KeyRecord !=
			       goalToken.KeyRecord)
			{
				MoveArrowRight(nextPlainTextEditorState, new KeyDownEventRecord(
					KeyboardFacts.MovementKeys.ARROW_RIGHT_KEY,
					KeyboardFacts.MovementKeys.ARROW_RIGHT_KEY,
					false,
					false,
					false));
			}

			while (nextPlainTextEditorState.CurrentPlainTextToken.IndexInPlainText !=
			       nextPlainTextEditorState.CurrentPlainTextToken.ToPlainText.Length - 1)
			{
				MoveArrowRight(nextPlainTextEditorState, new KeyDownEventRecord(
					KeyboardFacts.MovementKeys.ARROW_RIGHT_KEY,
					KeyboardFacts.MovementKeys.ARROW_RIGHT_KEY,
					false,
					false,
					false));
			}
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

			foreach (var token in nextPlainTextEditorState.CurrentRow.Items)
			{
				if (token.KeyRecord == nextPlainTextEditorState.CurrentPlainTextToken.KeyRecord)
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

		private static PlainTextTokenMetaData? CalculateCurrentTokenColumnIndexRespectiveToDocument(
			PlainTextEditorState nextPlainTextEditorState)
		{
			var rollingCount = 0;

			foreach (var rowKey in nextPlainTextEditorState.FeatureStateKeys)
			{
				var row = nextPlainTextEditorState[rowKey];

				foreach (var token in row.Items)
				{
					if (token.KeyRecord == nextPlainTextEditorState.CurrentPlainTextToken.KeyRecord)
					{
						return new PlainTextTokenMetaData(token,
							null,
							new PositionSpanRelativeToDocumentRecord(rollingCount,
								rollingCount + token.ToPlainText.Length));
					}
					else
					{
						rollingCount += token.ToPlainText.Length;
					}
				}
			}

			return null;
		}

		private static PlainTextTokenMetaData CalculateTokenAtColumnIndexRespectiveToRow(
			PlainTextEditorState nextPlainTextEditorState,
			PlainTextRow row,
			int columnIndex)
		{
			var rollingCount = 0;
			PlainTextTokenBase? token = row[0];

			foreach (var temporaryToken in row.Items)
			{
				token = temporaryToken;

				rollingCount += token.ToPlainText.Length;

				if (rollingCount > columnIndex)
					break;
			}

			var positionRelativeToRow = new PositionSpanRelativeToRowRecord(
				rollingCount - token.ToPlainText.Length,
				rollingCount);

			return new PlainTextTokenMetaData(token, positionRelativeToRow, null);
		}

		private static void PerformBackspace(PlainTextEditorState nextPlainTextEditorState,
			KeyDownEventRecord keyDownEventRecord)
		{
			if (nextPlainTextEditorState.CurrentPlainTextToken.KeyRecord ==
			    nextPlainTextEditorState[0][0].KeyRecord)
			{
				// Do not allow Backspace to remove the First().First() StartOfRow token.
				return;
			}

			var removeAStartOfRow = nextPlainTextEditorState.CurrentPlainTextToken.PlainTextTokenKind ==
			                        PlainTextTokenKind.StartOfRow;
			var temporarilyStoredTokenKeyIndex = nextPlainTextEditorState.CurrentPlainTextTokenKeyIndex;

			var nextRow = nextPlainTextEditorState.CurrentRow
				.WithRemove(nextPlainTextEditorState.CurrentPlainTextToken.KeyRecord);

			nextPlainTextEditorState.FeatureStateMap[nextRow.KeyRecord] = nextRow;

			SetPreviousTokenAsCurrentToken(nextPlainTextEditorState, keyDownEventRecord, false);

			if (removeAStartOfRow)
			{
				MoveRowToEndOfOtherRow(nextPlainTextEditorState,
					nextRow,
					nextPlainTextEditorState.CurrentRow);
			}
			else
			{
				var temporarilyStoredToken = nextPlainTextEditorState.CurrentPlainTextToken;

				nextPlainTextEditorState.FeatureStateMap[nextRow.KeyRecord] =
					PlainTextRow.PerformMergingOn(nextRow, temporarilyStoredTokenKeyIndex - 1);

				SetIndexInPlainTextOfCurrentToken(nextPlainTextEditorState, temporarilyStoredToken.IndexInPlainText);
			}
		}

		/// <summary>
		/// Joins together two rows into one.
		/// </summary>
		private static void MoveRowToEndOfOtherRow(PlainTextEditorState nextPlainTextEditorState,
			PlainTextRow plainTextRowToBeMoved,
			PlainTextRow plainTextRowOther)
		{
			var nextRow = plainTextRowOther.WithAddRange(plainTextRowToBeMoved);

			nextPlainTextEditorState.FeatureStateMap[nextRow.KeyRecord] = nextRow;

			nextPlainTextEditorState.FeatureStateMap.Remove(plainTextRowToBeMoved.KeyRecord);
			nextPlainTextEditorState.FeatureStateKeys.Remove(plainTextRowToBeMoved.KeyRecord);
		}

		private static void HandleWhitespace(PlainTextEditorState nextPlainTextEditorState,
			KeyDownEventRecord keyDownEventRecord)
		{
			if (KeyboardFacts.WhitespaceKeys.ENTER_CODE == keyDownEventRecord.Code)
			{
				if (nextPlainTextEditorState.CurrentPlainTextTokenKeyIndex !=
				    nextPlainTextEditorState.CurrentRow.Count)
				{
					PlainTextRow nextRow = nextPlainTextEditorState.CurrentRow;
					PlainTextRow createdRowBelow = new PlainTextRow();

					if (nextPlainTextEditorState.CurrentPlainTextToken.PlainTextTokenKind ==
					    PlainTextTokenKind.Default &&
					    nextPlainTextEditorState.CurrentPlainTextToken.IndexInPlainText <
					    nextPlainTextEditorState.CurrentPlainTextToken.ToPlainText.Length - 1)
					{
						nextRow = nextPlainTextEditorState.CurrentRow
							.WithRemove(nextPlainTextEditorState.CurrentPlainTextToken.KeyRecord);

						var tokenFirstString = nextPlainTextEditorState.CurrentPlainTextToken.ToPlainText
							.Substring(0,
								nextPlainTextEditorState.CurrentPlainTextToken.IndexInPlainText!.Value + 1);

						var tokenSecondString = nextPlainTextEditorState.CurrentPlainTextToken.ToPlainText
							.Substring(nextPlainTextEditorState.CurrentPlainTextToken.IndexInPlainText!.Value + 1);

						var tokenFirst = new DefaultPlainTextToken(tokenFirstString);
						var tokenSecond = new DefaultPlainTextToken(tokenSecondString);
						
						nextRow = nextRow.WithInsert(tokenFirst.KeyRecord,
							tokenFirst,
							nextPlainTextEditorState.CurrentPlainTextTokenKeyIndex);

						nextRow = nextRow.WithInsert(tokenSecond.KeyRecord,
							tokenSecond,
							nextPlainTextEditorState.CurrentPlainTextTokenKeyIndex + 1);
					}

					var createdRowBelowInsertIndex = 1;

					for (var i = nextPlainTextEditorState.CurrentPlainTextTokenKeyIndex + 1;
					     i < nextRow.Count;
					     i++)
					{
						var token = nextRow[i];

						nextRow = nextRow.WithRemove(token.KeyRecord);
						createdRowBelow = createdRowBelow.WithInsert(token.KeyRecord, 
							token, 
							createdRowBelowInsertIndex++);

						i--;
					}

					nextPlainTextEditorState.FeatureStateMap[nextRow.KeyRecord] = nextRow;

					SetIndexInPlainTextOfCurrentToken(nextPlainTextEditorState, null);

					nextPlainTextEditorState.FeatureStateKeys.Insert(nextPlainTextEditorState.CurrentRowIndex + 1,
						createdRowBelow.KeyRecord);

					nextPlainTextEditorState.FeatureStateMap.Add(createdRowBelow.KeyRecord, createdRowBelow);
				}

				nextPlainTextEditorState.CurrentRowIndex++;

				nextPlainTextEditorState.CurrentPlainTextTokenKeyIndex = 0;
			}
			else
			{
				SetIndexInPlainTextOfCurrentToken(nextPlainTextEditorState, null);

				var whitespaceToken = new WhitespacePlainTextToken(keyDownEventRecord);

				var nextRow = nextPlainTextEditorState.CurrentRow.ConstructDeepClone();

				nextRow = nextRow.WithInsert(whitespaceToken.KeyRecord,
					whitespaceToken,
					nextPlainTextEditorState.CurrentPlainTextTokenKeyIndex + 1);

				nextPlainTextEditorState.FeatureStateMap[nextRow.KeyRecord] = nextRow;

				nextPlainTextEditorState.CurrentPlainTextTokenKeyIndex += 1;
			}
		}

		private static void PerformTextSelection(PlainTextEditorState nextPlainTextEditorState,
			int columnIndexRelativeToDocument,
			int offsetDisplacementChange,
			SelectionDirectionBinding selectionDirectionBinding)
		{
			if (nextPlainTextEditorState.SelectionSpanRecord is null)
			{
				var inclusiveStartingColumnIndexRelativeToDocument = selectionDirectionBinding switch {
					SelectionDirectionBinding.Left => columnIndexRelativeToDocument,
					SelectionDirectionBinding.Right => columnIndexRelativeToDocument + 1,
					_ => throw new ApplicationException($"The {nameof(SelectionDirectionBinding)} " +
					                                    $"{selectionDirectionBinding} is not currently supported.")
				};

				nextPlainTextEditorState.SelectionSpanRecord =
					new SelectionSpanRecord(inclusiveStartingColumnIndexRelativeToDocument,
						0,
						selectionDirectionBinding);
			}
			else
			{
				nextPlainTextEditorState.SelectionSpanRecord = new SelectionSpanRecord(
					nextPlainTextEditorState.SelectionSpanRecord.InclusiveStartingColumnIndexRelativeToDocument,
					nextPlainTextEditorState.SelectionSpanRecord.OffsetDisplacement + offsetDisplacementChange,
					nextPlainTextEditorState.SelectionSpanRecord.InitialDirectionBinding);

				if (nextPlainTextEditorState.SelectionSpanRecord.OffsetDisplacement == 0)
				{
					nextPlainTextEditorState.SelectionSpanRecord = null;
				}
			}
		}
	}
}