using Fluxor;
using FluxorExperiments.ClassLibrary.KeyDownEvent;
using FluxorExperiments.ClassLibrary.PlainTextEditor;
using System.Collections.Immutable;

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
	
	public int CurrentRowIndex { get; private set; }
	public int CurrentPlainTextTokenKeyIndex { get; private set; }
	public PlainTextRow CurrentRow => GetCurrentRow();
	public PlainTextTokenBase CurrentPlainTextToken => GetCurrentPlainTextToken();
	public ImmutableArray<PlainTextRowKey> PlainTextRowKeys => _plainTextRowKeys.ToImmutableArray();

	private PlainTextTokenBase GetCurrentPlainTextToken()
	{
		return CurrentRow.GetPlainTextTokenFromKeyAtIndex(CurrentPlainTextTokenKeyIndex);
	}

	private PlainTextRow GetCurrentRow()
	{
		return _plainTextRowMap[_plainTextRowKeys[CurrentRowIndex]];
	}
}