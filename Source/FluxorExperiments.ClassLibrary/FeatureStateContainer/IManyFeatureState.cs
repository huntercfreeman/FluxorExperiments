namespace FluxorExperiments.ClassLibrary.FeatureStateContainer;

public interface IManyFeatureState<TKey, TItem>
    where TKey : KeyRecord
{
    public SequenceKeyRecord SequenceKeyRecord { get; }
    public TKey KeyRecord { get; set; }
    
    public TItem ConstructDeepClone();
}