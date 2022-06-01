using Fluxor;
using System.Collections.Immutable;

namespace FluxorExperiments.ClassLibrary.FeatureStateContainer;

public record FeatureStateContainerRecord<TFeatureStateContainerRecord, TKey, TItem>
    where TFeatureStateContainerRecord : FeatureStateContainerRecord<TFeatureStateContainerRecord, TKey, TItem>
    where TKey : KeyRecord
    where TItem : IManyFeatureState<TKey, TItem>
{
    private Dictionary<TKey, TItem> _featureStateMap;
    private List<TKey> _featureStateKeys;

    /// <summary>
    /// Only Fluxor should be calling this constructor
    /// </summary>
    protected FeatureStateContainerRecord()
    {
        _featureStateMap = new();
        _featureStateKeys = new();
    }

    protected virtual TFeatureStateContainerRecord ConstructDeepClone()
    {
        // The state must be immutable in order for this shallow copy
        // to behave in effect like a deep copy.
        return (TFeatureStateContainerRecord) this with { };
    }


    public virtual TFeatureStateContainerRecord WithAdd(TKey key, TItem item)
    {
        var otherFeatureStateContainerRecord = ConstructDeepClone();

        otherFeatureStateContainerRecord._featureStateKeys.Add(key);
        otherFeatureStateContainerRecord._featureStateMap.Add(key, item);

        return otherFeatureStateContainerRecord;
    }
    
    public virtual TFeatureStateContainerRecord WithInsert(TKey key, TItem item, int index)
    {
        var otherFeatureStateContainerRecord = ConstructDeepClone();

        otherFeatureStateContainerRecord._featureStateKeys.Insert(index, key);
        otherFeatureStateContainerRecord._featureStateMap.Add(key, item);

        return otherFeatureStateContainerRecord;
    }
    
    public virtual TFeatureStateContainerRecord WithRemove(TKey key)
    {
        var otherFeatureStateContainerRecord = ConstructDeepClone();

        otherFeatureStateContainerRecord._featureStateKeys.Add(key);
        otherFeatureStateContainerRecord._featureStateMap.Remove(key);

        return otherFeatureStateContainerRecord;
    }
    
    public virtual TFeatureStateContainerRecord WithReplace(TKey key, TItem item)
    {
        var otherFeatureStateContainerRecord = ConstructDeepClone();

        otherFeatureStateContainerRecord._featureStateMap[key] = item;

        return otherFeatureStateContainerRecord;
    }

    public ImmutableArray<TItem> Items => _featureStateKeys
        .Select(key => _featureStateMap[key])
        .ToImmutableArray();

    public TItem this[TKey key] => _featureStateMap[key];
}