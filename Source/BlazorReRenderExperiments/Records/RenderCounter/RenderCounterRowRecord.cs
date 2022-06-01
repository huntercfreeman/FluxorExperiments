using System.Collections.Immutable;

namespace BlazorReRenderExperiments.Records.RenderCounter;

public record RenderCounterRowRecord
{
	private Dictionary<RenderCounterRecordKey, RenderCounterRecord> _renderCounterRecordMap;
	private List<RenderCounterRecordKey> _renderCounterRecordKeys;

	public RenderCounterRowRecord()
	{
		_renderCounterRecordMap = new();
		_renderCounterRecordKeys = new();
	}
	
	private RenderCounterRowRecord ConstructDeepClone()
	{
		var cloneRenderCounterRowRecord = new RenderCounterRowRecord();

		cloneRenderCounterRowRecord._renderCounterRecordMap = 
			new(_renderCounterRecordMap);

		return cloneRenderCounterRowRecord;
	}

	public RenderCounterRowRecord WithAdd(RenderCounterRecordKey renderCounterRecordKey)
	{
		var clonedRenderCounterRowRecord = ConstructDeepClone();

		clonedRenderCounterRowRecord._renderCounterRecordMap.Remove(renderCounterRecordKey);

		return clonedRenderCounterRowRecord;
	}
	
	public RenderCounterRowRecord WithRemove(RenderCounterRecordKey renderCounterRecordKey)
	{
		var clonedRenderCounterRowRecord = ConstructDeepClone();

		clonedRenderCounterRowRecord._renderCounterRecordMap.Remove(renderCounterRecordKey);

		return clonedRenderCounterRowRecord;
	}
	
	public RenderCounterRowRecord WithReplace(RenderCounterRecordKey renderCounterRecordKey)
	{
		var clonedRenderCounterRowRecord = ConstructDeepClone();

		clonedRenderCounterRowRecord._renderCounterRecordMap.Remove(renderCounterRecordKey);

		return clonedRenderCounterRowRecord;
	}

	public int RenderCounterRecordsCount =>
		_renderCounterRecordMap.Count;
	public ImmutableArray<RenderCounterRecord> RenderCounterRecords => 
		_renderCounterRecordMap.Values.ToImmutableArray();
}