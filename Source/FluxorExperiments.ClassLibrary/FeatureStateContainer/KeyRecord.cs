namespace FluxorExperiments.ClassLibrary.FeatureStateContainer;

public abstract record KeyRecord
{
    public Guid Id { get; } = Guid.NewGuid();
}