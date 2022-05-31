using Fluxor;
using FluxorExperiments.ClassLibrary.KeyDownEvent;
using FluxorExperiments.ClassLibrary.PlainTextEditor;
using System.Collections.Immutable;
using System.Text;

namespace FluxorExperiments.ClassLibrary.Store.PlainTextEditor;

[FeatureState]
public partial record PlainTextEditorState
{
	private readonly Dictionary<PlainTextRowKey, PlainTextRow> _plainTextRowMap;
	private readonly List<PlainTextRowKey> _plainTextRowKeys;

	public PlainTextEditorState()
	{
		var row = new PlainTextRow();
		
		_plainTextRowMap = new() 
		{
			{ row.PlainTextRowKey, row }
		};
		
		_plainTextRowKeys = new() 
		{
			row.PlainTextRowKey		
		};
	}

	protected PlainTextEditorState(PlainTextEditorState otherPlainTextEditorState)
	{
		_plainTextRowMap = 
			new(otherPlainTextEditorState._plainTextRowMap);
		
		_plainTextRowKeys = new(otherPlainTextEditorState._plainTextRowKeys);

		SelectionSpanRecord = otherPlainTextEditorState.SelectionSpanRecord;

		CurrentRowIndex = otherPlainTextEditorState.CurrentRowIndex;
		CurrentPlainTextTokenKeyIndex = otherPlainTextEditorState.CurrentPlainTextTokenKeyIndex;
	}

	public PlainTextRow LookupPlainTextRow(PlainTextRowKey plainTextRowKey)
	{
		return _plainTextRowMap[plainTextRowKey];
	}

	public PlainTextEditorState GetNextState(KeyDownEventRecord keyDownEventRecord)
	{
		return PlainTextEditorStateMachine.GetNextState(this, keyDownEventRecord);
	}
	
	/// <summary>
	/// Specify a specific rowIndex, plainTextTokenKeyIndex, and characterIndex
	/// and that location will be set as the current token.
	/// </summary>
	/// <param name="rowIndex"></param>
	/// <param name="plainTextTokenKeyIndex"></param>
	/// <param name="characterIndex"></param>
	/// <returns></returns>
	/// <exception cref="NotImplementedException"></exception>
	public PlainTextEditorState GetNextState(int rowIndex, int plainTextTokenKeyIndex, int characterIndex)
	{
		return PlainTextEditorStateMachine.GetNextState(this, rowIndex, plainTextTokenKeyIndex, characterIndex);
	}
	
	public int CurrentRowIndex { get; private set; }
	public int CurrentPlainTextTokenKeyIndex { get; private set; }
	public SelectionSpanRecord? SelectionSpanRecord { get; private set; }
	public PlainTextRow CurrentRow => GetCurrentRow();
	public PlainTextTokenBase CurrentPlainTextToken => GetCurrentPlainTextToken();
	public ImmutableArray<PlainTextRowKey> PlainTextRowKeys => _plainTextRowKeys.ToImmutableArray();
	public int RowCount => _plainTextRowKeys.Count;
	public string EntireDocumentToPlainText => GetEntireDocumentToPlainText();

	public PlainTextRow GetRowAtIndex(int index)
	{
		return _plainTextRowMap[_plainTextRowKeys[index]];
	}

	private PlainTextTokenBase GetCurrentPlainTextToken()
	{
		return CurrentRow.GetPlainTextTokenFromIndex(CurrentPlainTextTokenKeyIndex);
	}

	private PlainTextRow GetCurrentRow()
	{
		return _plainTextRowMap[_plainTextRowKeys[CurrentRowIndex]];
	}

	private string GetEntireDocumentToPlainText()
	{
		var plainTextBuilder = new StringBuilder();

		var firstStartOfRowToken = GetRowAtIndex(0).GetPlainTextTokenFromIndex(0);

		foreach (var rowKey in _plainTextRowKeys)
		{
			var row = LookupPlainTextRow(rowKey);

			foreach (var tokenKey in row.PlainTextTokenKeys)
			{
				if (tokenKey == firstStartOfRowToken.PlainTextTokenKey)
				{
					continue;
				}
				
				var token = row.LookupPlainTextToken(tokenKey);

				plainTextBuilder.Append(token.ToPlainText);
			}
		}

		return plainTextBuilder.ToString();
	}
}