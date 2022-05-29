using FluxorExperiments.Classes.Sequence;

namespace FluxorExperiments.Classes.PlainTextEditor;

public record PlainTextTokenKey(Guid Id)
{
	public static PlainTextTokenKey NewPlainTextTokenKey()
	{
		return new PlainTextTokenKey(Guid.NewGuid());
	}
}

public record PlainTextTokenBase(PlainTextTokenKey PlainTextTokenKey, SequenceKey SequenceKey)
{
}

public record DefaultPlainTextTokenBase(PlainTextTokenKey PlainTextTokenKey, SequenceKey SequenceKey) 
	: PlainTextTokenBase(PlainTextTokenKey, SequenceKey)
{
	
}

public record WhitespacePlainTextTokenBase(PlainTextTokenKey PlainTextTokenKey, SequenceKey SequenceKey) 
	: PlainTextTokenBase(PlainTextTokenKey, SequenceKey)
{
	
}

public record StartOfRowPlainTextTokenBase(PlainTextTokenKey PlainTextTokenKey, SequenceKey SequenceKey) 
	: PlainTextTokenBase(PlainTextTokenKey, SequenceKey)
{
	
}
