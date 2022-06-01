namespace FluxorExperiments.ClassLibrary.FeatureStateContainer;

public record SequenceKeyRecord
{
    public Guid Id { get; } = Guid.NewGuid();
}