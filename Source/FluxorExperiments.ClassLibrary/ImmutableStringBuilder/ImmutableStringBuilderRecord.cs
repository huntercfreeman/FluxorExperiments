using System.Text;

namespace FluxorExperiments.ClassLibrary.ImmutableStringBuilder;

public record ImmutableStringBuilderRecord
{
	private readonly StringBuilder _stringBuilder;

	public ImmutableStringBuilderRecord()
	{
		_stringBuilder = new();
	}
	
	public void Insert(int index, string value)
	{
		_stringBuilder.Insert(index, value);
	}

	public void Insert(int index, ReadOnlySpan<char> value)
	{
		_stringBuilder.Insert(index, value);
	}

	public void RemoveAt(int index)
	{
		_stringBuilder.Remove(index, 1);
	}

	public ReadOnlySpan<char> ToStringAsSpan(ImmutableStringBuilderRecordKey immutableStringBuilderRecordKey)
	{
		int length = immutableStringBuilderRecordKey.Length;
		Span<char> span = new char[length];
		_stringBuilder.CopyTo(0, span, length);
		return span;
	}


	//Raymond: Why did i try to implment this myself? I don't know.
	//public ReadOnlySpan<char> ToStringAsSpan(ImmutableStringBuilderRecordKey immutableStringBuilderRecordKey)
	//{
	//	bool first = true;
	//	int index = 0;
	//	int lengthNeeds = immutableStringBuilderRecordKey.Length;
	//	Span<char> span = Span<char>.Empty;
		
	//	foreach (ReadOnlyMemory<char> chunk in _stringBuilder.GetChunks())
	//	{
	//		if (first && chunk.Length == lengthNeeds) return chunk.Span;
	//		if (first && chunk.Length > lengthNeeds) return chunk.Span[..lengthNeeds];
	//		first = false;
	//		if (span.IsEmpty) span = new char[lengthNeeds];
	//		if (chunk.Length == lengthNeeds)
	//		{
	//			chunk.Span.CopyTo(span.Slice(index, lengthNeeds));
	//		}
	//		else if (chunk.Length > lengthNeeds)
	//		{		
	//			chunk.Span[..lengthNeeds].CopyTo(span.Slice(index, lengthNeeds));
	//		}
	//		else if (chunk.Length < lengthNeeds)
	//		{
	//			chunk.Span.CopyTo(span.Slice(index, lengthNeeds));
	//		}
	//		lengthNeeds -= chunk.Length;
	//		if (lengthNeeds <= 0) break;
	//		index += chunk.Length;
	//	}
	//	return span;
	//}
}