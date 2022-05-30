using FluxorExperiments.ClassLibrary.Sequence;
using System.Collections.Immutable;

namespace FluxorExperiments.ClassLibrary.PlainTextEditor;

public record PlainTextRowKey(Guid Id)
{
	public static PlainTextRowKey NewPlainTextRowKey()
	{
		return new PlainTextRowKey(Guid.NewGuid());
	}
}

public record PlainTextRow(PlainTextRowKey PlainTextRowKey, bool IsActiveRow, SequenceKey SequenceKey)
{
	private readonly Dictionary<PlainTextTokenKey, PlainTextTokenBase> _plainTextTokenMap;
	private readonly List<PlainTextTokenKey> _plainTextTokenKeys;

	public PlainTextRow() : this(PlainTextRowKey.NewPlainTextRowKey(), true, SequenceKey.NewSequenceKey())
	{
		var startOfRowToken = new StartOfRowPlainTextToken();

		_plainTextTokenMap = new() {
			{ startOfRowToken.PlainTextTokenKey, startOfRowToken }
		};

		_plainTextTokenKeys = new() {
			startOfRowToken.PlainTextTokenKey
		};

		PlainTextRowKey = PlainTextRowKey.NewPlainTextRowKey();
		SequenceKey = SequenceKey.NewSequenceKey();
	}

	public PlainTextRow(PlainTextRow otherPlainTextRow)
	{
		_plainTextTokenMap = new(otherPlainTextRow._plainTextTokenMap);
		_plainTextTokenKeys = new(otherPlainTextRow._plainTextTokenKeys);

		PlainTextRowKey = otherPlainTextRow.PlainTextRowKey;
		SequenceKey = SequenceKey.NewSequenceKey();
	}

	public object this[int i]
	{
		get { return _plainTextTokenKeys[i]; }
	}

	public PlainTextRow WithInsert(PlainTextTokenBase plainTextToken,
		int index)
	{
		var nextPlainTextRow = new PlainTextRow(this);

		nextPlainTextRow._plainTextTokenMap.Add(plainTextToken.PlainTextTokenKey, plainTextToken);
		nextPlainTextRow._plainTextTokenKeys.Insert(index, plainTextToken.PlainTextTokenKey);

		return nextPlainTextRow;
	}

	public PlainTextRow WithReplace(PlainTextTokenKey plainTextTokenKey,
		PlainTextTokenBase plainTextToken)
	{
		var nextPlainTextRow = new PlainTextRow(this);

		nextPlainTextRow._plainTextTokenMap[plainTextTokenKey] = plainTextToken;

		return nextPlainTextRow;
	}

	// TODO: Because the state is immutable we can always reliable store the index that the key resides at to speed removal without having to search for the key in the list of keys
	public PlainTextRow WithRemove(PlainTextTokenKey plainTextTokenKey)
	{
		var nextPlainTextRow = new PlainTextRow(this);

		nextPlainTextRow._plainTextTokenMap.Remove(plainTextTokenKey);

		nextPlainTextRow._plainTextTokenKeys.Remove(plainTextTokenKey);

		return nextPlainTextRow;
	}

	public PlainTextRow WithAddRange(PlainTextRow otherPlainTextRow)
	{
		var currentCount = TokenCount;

		var nextPlainTextRow = new PlainTextRow(this);

		nextPlainTextRow._plainTextTokenKeys.AddRange(otherPlainTextRow._plainTextTokenKeys);

		foreach (var mapping in otherPlainTextRow._plainTextTokenMap)
		{
			nextPlainTextRow._plainTextTokenMap.Add(mapping.Key, mapping.Value);
		}

		if (currentCount != nextPlainTextRow.TokenCount)
			return PerformMergingOn(nextPlainTextRow, currentCount - 1);

		return nextPlainTextRow;
	}

	private PlainTextRow WithRemoveRange(IEnumerable<PlainTextTokenKey> tokenKeys)
	{
		var nextPlainTextRow = new PlainTextRow(this);

		foreach (var tokenKey in tokenKeys)
		{
			nextPlainTextRow._plainTextTokenMap.Remove(tokenKey);
			nextPlainTextRow._plainTextTokenKeys.Remove(tokenKey);
		}

		return nextPlainTextRow;
	}

	public static PlainTextRow PerformMergingOn(PlainTextRow plainTextRow,
		int startingIndex = 0)
	{
		// A token was added therefore we possibly have
		// two DefaultPlainTextTokens "side by side" and need to merge them into one
		for (int i = startingIndex; i < plainTextRow.TokenCount - 1; i++)
		{
			var tokenFirstKey = plainTextRow._plainTextTokenKeys[i];
			var tokenSecondKey = plainTextRow._plainTextTokenKeys[i + 1];

			var mergeToken = PlainTextTokenMerger
				.MergePlainTextTokens(plainTextRow.LookupPlainTextToken(tokenFirstKey),
					plainTextRow.LookupPlainTextToken(tokenSecondKey));

			if (mergeToken is not null)
			{
				plainTextRow = plainTextRow.WithRemoveRange(new PlainTextTokenKey[] {
					tokenFirstKey,
					tokenSecondKey
				});

				plainTextRow = plainTextRow.WithInsert(mergeToken,
					i);

				// One must revisit index i for a second time if the list changes
				i--;
			}
		}

		return plainTextRow;
	}

	public ImmutableArray<PlainTextTokenKey> PlainTextTokenKeys => _plainTextTokenKeys.ToImmutableArray();
	public int TokenCount => _plainTextTokenKeys.Count;

	public PlainTextTokenBase LookupPlainTextToken(PlainTextTokenKey plainTextTokenKey)
	{
		return _plainTextTokenMap[plainTextTokenKey];
	}

	public PlainTextTokenBase GetPlainTextTokenFromIndex(int plainTextTokenKeyIndex)
	{
		return _plainTextTokenMap[_plainTextTokenKeys[plainTextTokenKeyIndex]];
	}
}