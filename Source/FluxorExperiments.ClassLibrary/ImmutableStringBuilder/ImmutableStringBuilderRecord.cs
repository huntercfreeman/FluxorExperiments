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
	
	public ReadOnlySpan<char> ToStringSpan(ImmutableStringBuilderRecordKey immutableStringBuilderRecordKey)
	{
		return _stringBuilder
			.ToString().AsSpan()[..immutableStringBuilderRecordKey.Length];
	}
}