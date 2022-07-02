namespace FluxorExperiments.ClassLibrary.FeatureStateContainer;

public interface IManyFeatureState<TKey, TItem>
    where TKey : struct, IKeyRecord
{
    public SequenceKeyRecord SequenceKeyRecord { get; }
    public TKey KeyRecord { get; init; }
    
    public TItem ConstructDeepClone();
}