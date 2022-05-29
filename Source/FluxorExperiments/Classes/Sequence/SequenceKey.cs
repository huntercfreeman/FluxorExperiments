namespace FluxorExperiments.Classes.Sequence;

// Used to know whether a Blazor Component should re-render
public record SequenceKey(Guid Id)
{
	public static SequenceKey NewSequenceKey()
	{
		return new SequenceKey(Guid.NewGuid());
	}
}