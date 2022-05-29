namespace FluxorExperiments.ClassLibrary.PlainTextEditor;

public record PlainTextTokenKey(Guid Id)
{
	public static PlainTextTokenKey NewPlainTextTokenKey()
	{
		return new PlainTextTokenKey(Guid.NewGuid());
	}
}