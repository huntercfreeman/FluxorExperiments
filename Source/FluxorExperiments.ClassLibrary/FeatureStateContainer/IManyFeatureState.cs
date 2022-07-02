namespace FluxorExperiments.ClassLibrary.FeatureStateContainer;

public interface IManyFeatureState<TKey, TItem>
    where TKey : struct, IKeyRecord
{
    public SequenceKeyRecord SequenceKeyRecord { get; init; }
    public TKey KeyRecord { get; init; }
    
    public TItem ConstructDeepClone();
}