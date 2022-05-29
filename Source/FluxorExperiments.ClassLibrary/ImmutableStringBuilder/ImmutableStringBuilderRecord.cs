using System.Text;

namespace FluxorExperiments.ClassLibrary.ImmutableStringBuilder;

public record ImmutableStringBuilderRecord
{
	private readonly StringBuilder _stringBuilder;

	public ImmutableStringBuilderRecord()
	{
		_stringBuilder = new();
	}
	
	public ImmutableStringBuilderRecordKey Append(string value)
	{
		_stringBuilder.Append(value);

		return new ImmutableStringBuilderRecordKey(_stringBuilder.Length);
	}
	
	public string GetString(ImmutableStringBuilderRecordKey immutableStringBuilderRecordKey)
	{
		return _stringBuilder
			.ToString()[..immutableStringBuilderRecordKey.Length];
	}
}

public record ImmutableStringBuilderRecordKey(int Length);