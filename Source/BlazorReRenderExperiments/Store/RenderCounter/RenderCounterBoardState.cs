using BlazorReRenderExperiments.Records.RenderCounter;

namespace BlazorReRenderExperiments.Store.RenderCounter;

public record RenderCounterBoardState
{
	private Dictionary<RenderCounterRowRecordKey, RenderCounterRowRecord> _renderCounterRowMap;
	private List<RenderCounterRowRecordKey> _renderCounterRowRecordKeys;

	public RenderCounterBoardState()
	{
		_renderCounterRowMap = new();
		_renderCounterRowRecordKeys = new();
	}

	private RenderCounterBoardState ConstructDeepClone()
	{
		var cloneRenderCounterBoardState = new RenderCounterBoardState();

		cloneRenderCounterBoardState._renderCounterRowMap = 
			new(_renderCounterRowMap);
		
		cloneRenderCounterBoardState._renderCounterRowRecordKeys = 
			new(_renderCounterRowRecordKeys);

		return cloneRenderCounterBoardState;
	}

	public RenderCounterBoardState WithAdd(RenderCounterRowRecordKey renderCounterRowRecordKey,
		RenderCounterRecordKey renderCounterRecordKey)
	{
		var clonedRenderCounterBoardState = ConstructDeepClone();

		clonedRenderCounterBoardState._renderCounterRowRecordKeys.Add(renderCounterRowRecordKey);
		
		clonedRenderCounterBoardState._renderCounterRowMap.Add(renderCounterRowRecordKey,
			new());

		return clonedRenderCounterBoardState;
	}

	public RenderCounterBoardState WithRemove(RenderCounterRowRecordKey renderCounterRowRecordKey, RenderCounterRecordKey renderCounterRecordKey)
	{
		var clonedRenderCounterBoardState = ConstructDeepClone();

		var previousRow = clonedRenderCounterBoardState._renderCounterRowMap[renderCounterRowRecordKey];

		var nextRow = previousRow.WithRemove(renderCounterRecordKey);

		if (nextRow.RenderCounterRecordsCount == 0)
		{
			clonedRenderCounterBoardState._renderCounterRowRecordKeys.Remove(renderCounterRowRecordKey);
			clonedRenderCounterBoardState._renderCounterRowMap.Remove(renderCounterRowRecordKey);
		}
		else
		{
			clonedRenderCounterBoardState._renderCounterRowMap[renderCounterRowRecordKey] =
				nextRow;
		}

		return clonedRenderCounterBoardState;
	}

	public RenderCounterBoardState WithReplace(RenderCounterRowRecordKey renderCounterRowRecordKey, 
		RenderCounterRecordKey renderCounterRecordKey)
	{
		throw new NotImplementedException();
	}
}