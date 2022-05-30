namespace FluxorExperiments.ClassLibrary.PlainTextEditor;

public static class PlainTextTokenMerger
{
	public static PlainTextTokenBase? MergePlainTextTokens(PlainTextTokenBase tokenFirst,
		PlainTextTokenBase tokenSecond)
	{
		return tokenFirst.PlainTextTokenKind switch 
		{
			PlainTextTokenKind.StartOfRow => HandleTokenFirstOfKindStartOfRow(tokenFirst, tokenSecond),
			PlainTextTokenKind.Default => HandleTokenFirstOfKindDefault(tokenFirst, tokenSecond), 
			PlainTextTokenKind.Whitespace => HandleTokenFirstOfKindWhitespace(tokenFirst, tokenSecond), 
			_ => throw new ApplicationException($"The {nameof(PlainTextTokenKind)} of: " +
			                                    $"{tokenFirst.PlainTextTokenKind} " +
			                                    $"is not currently supported")
		};
	}

	private static PlainTextTokenBase? HandleTokenFirstOfKindStartOfRow(PlainTextTokenBase tokenFirst,
		PlainTextTokenBase tokenSecond)
	{
		return tokenSecond.PlainTextTokenKind switch
		{
			_ => null
		};
	}
	
	private static PlainTextTokenBase? HandleTokenFirstOfKindDefault(PlainTextTokenBase tokenFirst,
		PlainTextTokenBase tokenSecond)
	{
		return tokenSecond.PlainTextTokenKind switch
		{
			PlainTextTokenKind.Default => new DefaultPlainTextToken((DefaultPlainTextToken) tokenFirst, 
				(DefaultPlainTextToken) tokenSecond),
			_ => null
		};
	}
	
	private static PlainTextTokenBase? HandleTokenFirstOfKindWhitespace(PlainTextTokenBase tokenFirst,
		PlainTextTokenBase tokenSecond)
	{
		return tokenSecond.PlainTextTokenKind switch
		{
			_ => null
		};
	}
}