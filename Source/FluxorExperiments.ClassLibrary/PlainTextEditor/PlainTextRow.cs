﻿using FluxorExperiments.ClassLibrary.Sequence;
using System.Collections.Immutable;

namespace FluxorExperiments.ClassLibrary.PlainTextEditor;

public record PlainTextRowKey(Guid Id)
{
	public static PlainTextRowKey NewPlainTextRowKey()
	{
		return new PlainTextRowKey(Guid.NewGuid());
	}
}

public record PlainTextRow
{
	private readonly Dictionary<PlainTextTokenKey, PlainTextTokenBase> _plainTextTokenMap;
	private readonly List<PlainTextTokenKey> _plainTextTokenKeys;

	public PlainTextRow()
	{
		var startOfRowToken = new StartOfRowPlainTextToken();
		
		_plainTextTokenMap = new()
		{
			{ startOfRowToken.PlainTextTokenKey, startOfRowToken }
		};
		
		_plainTextTokenKeys = new()
		{
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

	public PlainTextRow WithInsert(PlainTextTokenKey plainTextTokenKey,
		PlainTextTokenBase plainTextToken,
		int index)
	{
		var nextPlainTextRow = new PlainTextRow(this);

		nextPlainTextRow._plainTextTokenMap.Add(plainTextTokenKey, plainTextToken);
		nextPlainTextRow._plainTextTokenKeys.Insert(index, plainTextTokenKey);

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
	public PlainTextRow WithRemove(PlainTextTokenKey plainTextTokenKey,
		PlainTextTokenBase plainTextToken)
	{
		var nextPlainTextRow = new PlainTextRow(this);

		nextPlainTextRow._plainTextTokenMap.Remove(plainTextTokenKey);

		// TODO: Prove this ignores memory reference and instead checks on value of inner Guid Id of the Key
		nextPlainTextRow._plainTextTokenKeys.Remove(plainTextTokenKey);

		return nextPlainTextRow;
	}

	public PlainTextRowKey PlainTextRowKey { get; private set; }
	public SequenceKey SequenceKey { get; private set; }
	public ImmutableArray<PlainTextTokenKey> PlainTextTokenKeys => _plainTextTokenKeys.ToImmutableArray();

	public PlainTextTokenBase LookupPlainTextToken(PlainTextTokenKey plainTextTokenKey)
	{
		return _plainTextTokenMap[plainTextTokenKey];
	}

	public PlainTextTokenBase GetPlainTextTokenFromKeyAtIndex(int plainTextTokenKeyIndex)
	{
		return _plainTextTokenMap[_plainTextTokenKeys[plainTextTokenKeyIndex]];
	}
}