using FluxorExperiments.ClassLibrary.FeatureStateContainer;
using System.Collections.Immutable;

namespace FluxorExperiments.ClassLibrary.PlainTextEditor;

public record PlainTextRow
	: FeatureStateContainerRecord<PlainTextRow, PlainTextTokenKey, PlainTextTokenBase>, 
		IManyFeatureState<PlainTextRowKey, PlainTextRow>
{
	public PlainTextRow()
	{
		var startOfRowToken = new StartOfRowPlainTextToken();
		
		FeatureStateKeys.Add(startOfRowToken.KeyRecord);
		FeatureStateMap.Add(startOfRowToken.KeyRecord, startOfRowToken);
	}
	
	public SequenceKeyRecord SequenceKeyRecord { get; init; } = new(Guid.NewGuid());
	public PlainTextRowKey KeyRecord { get; init; } = new(Guid.NewGuid());
	public bool IsActiveRow { get; init; }

	public override PlainTextRow ConstructDeepClone() => this with 
	{
		SequenceKeyRecord = new SequenceKeyRecord(Guid.NewGuid())
	};
	
	public PlainTextRow WithAddRange(PlainTextRow otherPlainTextRow)
	{
		var storedCount = Count;

		var nextPlainTextRow = new PlainTextRow(this);

		nextPlainTextRow.FeatureStateKeys.AddRange(otherPlainTextRow.FeatureStateKeys);

		foreach (var mapping in otherPlainTextRow.FeatureStateMap)
		{
			nextPlainTextRow.FeatureStateMap.Add(mapping.Key, mapping.Value);
		}

		if (storedCount != nextPlainTextRow.Count)
			return PerformMergingOn(nextPlainTextRow, storedCount - 1);

		return nextPlainTextRow;
	}

	private PlainTextRow WithRemoveRange(IEnumerable<PlainTextTokenKey> tokenKeys)
	{
		var nextPlainTextRow = new PlainTextRow(this);

		foreach (var tokenKey in tokenKeys)
		{
			nextPlainTextRow.FeatureStateMap.Remove(tokenKey);
			nextPlainTextRow.FeatureStateKeys.Remove(tokenKey);
		}

		return nextPlainTextRow;
	}

	public static PlainTextRow PerformMergingOn(PlainTextRow plainTextRow,
		int startingIndex = 0)
	{
		// A token was added therefore we possibly have
		// two DefaultPlainTextTokens "side by side" and need to merge them into one
		for (int i = startingIndex; i < plainTextRow.Count - 1; i++)
		{
			var tokenFirst = plainTextRow[i];
			var tokenSecond = plainTextRow[i + 1];

			var mergeToken = PlainTextTokenMerger
				.MergePlainTextTokens(tokenFirst,
					tokenSecond);

			if (mergeToken is not null)
			{
				plainTextRow = plainTextRow.WithRemoveRange(new[] {
					tokenFirst.KeyRecord,
					tokenSecond.KeyRecord
				});

				plainTextRow = plainTextRow.WithInsert(mergeToken.KeyRecord,
					mergeToken,
					i);

				// One must revisit index i for a second time if the list changes
				i--;
			}
		}

		return plainTextRow;
	}}