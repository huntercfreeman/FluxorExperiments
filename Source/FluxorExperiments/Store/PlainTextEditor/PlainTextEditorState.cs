using Fluxor;
using FluxorExperiments.Classes.PlainTextEditor;

namespace FluxorExperiments.Store.PlainTextEditor;

[FeatureState]
public record PlainTextEditorState
{
	private readonly Dictionary<PlainTextTokenKey, PlainTextTokenBase> _plainTextTokenMap;

	public PlainTextEditorState()
	{
		_plainTextTokenMap = new();
	}

	protected PlainTextEditorState(PlainTextEditorState otherPlainTextEditorState)
	{
		_plainTextTokenMap = 
			new(otherPlainTextEditorState._plainTextTokenMap);
	}

	public PlainTextTokenBase LookupPlainTextToken(PlainTextTokenKey plainTextTokenKey)
	{
		return _plainTextTokenMap[plainTextTokenKey];
	}
}

