using Fluxor;
using FluxorExperiments.ClassLibrary.Clipboard;
using FluxorExperiments.ClassLibrary.FeatureStateContainer;
using FluxorExperiments.ClassLibrary.KeyDownEvent;
using FluxorExperiments.ClassLibrary.PlainTextEditor;
using System.Collections.Immutable;
using System.Text;

namespace FluxorExperiments.ClassLibrary.Store.PlainTextEditor;

[FeatureState]
public partial record PlainTextEditorState
	: FeatureStateContainerRecord<PlainTextEditorState, PlainTextRowKey, PlainTextRow>
{
	public PlainTextEditorState()
	{
		var row = new PlainTextRow();

		FeatureStateKeys.Add(row.KeyRecord);
		FeatureStateMap.Add(row.KeyRecord, row);
	}
	
	public PlainTextEditorState GetNextState(KeyDownEventRecord keyDownEventRecord)
	{
		return PlainTextEditorStateMachine.GetNextState(this, keyDownEventRecord);
	}

	public PlainTextEditorState GetNextState(string bulkTextValue)
	{
		return PlainTextEditorStateMachine.GetNextState(this, bulkTextValue);
	}

	public PlainTextEditorState GetNextState(int rowIndex, int plainTextTokenKeyIndex, int characterIndex)
	{
		return PlainTextEditorStateMachine.GetNextState(this, rowIndex, plainTextTokenKeyIndex, characterIndex);
	}

	public int CurrentRowIndex { get; private set; }
	public int CurrentPlainTextTokenKeyIndex { get; private set; }
	public SelectionSpanRecord? SelectionSpanRecord { get; private set; }
	public PlainTextRow CurrentRow => this[CurrentRowIndex];
	public PlainTextTokenBase CurrentPlainTextToken => CurrentRow[CurrentPlainTextTokenKeyIndex];

	public string GetEntireDocumentToPlainText()
	{
		var plainTextBuilder = new StringBuilder();

		var firstStartOfRowToken = this[0][0];

		foreach (var rowKey in FeatureStateKeys)
		{
			var row = this[rowKey];

			foreach (var token in row.Items)
			{
				if (token.KeyRecord == firstStartOfRowToken.KeyRecord)
				{
					continue;
				}

				plainTextBuilder.Append(token.ToPlainText);
			}
		}

		return plainTextBuilder.ToString();	}
}