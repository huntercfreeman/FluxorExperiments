using Fluxor;
using FluxorExperiments.Classes.KeyDownEvent;
using FluxorExperiments.Classes.PlainTextEditor;

namespace FluxorExperiments.Store.PlainTextEditor;

public partial record PlainTextEditorState
{
	private static partial class PlainTextEditorStateMachine
	{
		public static PlainTextEditorState GetNextState(PlainTextEditorState plainTextEditorState, KeyDownEventRecord keyDownEventRecord)
		{
			var nextPlainTextEditorState = new PlainTextEditorState(plainTextEditorState);
			
			return nextPlainTextEditorState.CurrentPlainTextToken.PlainTextTokenKind switch 
			{
				PlainTextTokenKind.StartOfRow => GetNextStateFromStartOfRow(nextPlainTextEditorState, keyDownEventRecord),
				PlainTextTokenKind.Default => GetNextStateFromDefault(nextPlainTextEditorState, keyDownEventRecord),
				PlainTextTokenKind.Whitespace => GetNextStateFromWhitespace(nextPlainTextEditorState, keyDownEventRecord),
				_ => throw new ApplicationException($"The " +
				                                    $"{nameof(nextPlainTextEditorState.CurrentPlainTextToken.PlainTextTokenKind)} " +
				                                    $"of {nextPlainTextEditorState.CurrentPlainTextToken.PlainTextTokenKind} " +
				                                    $"is not currently supported.")
			};
		}
	}
}