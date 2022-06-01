using Fluxor;
using System.Collections.Immutable;

namespace FluxorExperiments.ClassLibrary.FeatureStateContainer;

public record FeatureStateContainerRecord<TFeatureStateContainerRecord, TKey, TItem>
    where TFeatureStateContainerRecord : FeatureStateContainerRecord<TFeatureStateContainerRecord, TKey, TItem>
    where TKey : KeyRecord
    where TItem : IManyFeatureState<TKey, TItem>
{
    protected readonly Dictionary<TKey, TItem> FeatureStateMap;
    protected readonly List<TKey> FeatureStateKeys;

    /// <summary>
    /// Only Fluxor should be calling this constructor
    /// </summary>
    protected FeatureStateContainerRecord()
    {
        FeatureStateMap = new();
        FeatureStateKeys = new();
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

        otherFeatureStateContainerRecord.FeatureStateKeys.Add(key);
        otherFeatureStateContainerRecord.FeatureStateMap.Add(key, item);

        return otherFeatureStateContainerRecord;
    }
    
    public virtual TFeatureStateContainerRecord WithInsert(TKey key, TItem item, int index)
    {
        var otherFeatureStateContainerRecord = ConstructDeepClone();

        otherFeatureStateContainerRecord.FeatureStateKeys.Insert(index, key);
        otherFeatureStateContainerRecord.FeatureStateMap.Add(key, item);

        return otherFeatureStateContainerRecord;
    }
    
    public virtual TFeatureStateContainerRecord WithRemove(TKey key)
    {
        var otherFeatureStateContainerRecord = ConstructDeepClone();

        otherFeatureStateContainerRecord.FeatureStateKeys.Add(key);
        otherFeatureStateContainerRecord.FeatureStateMap.Remove(key);

        return otherFeatureStateContainerRecord;
    }
    
    public virtual TFeatureStateContainerRecord WithReplace(TKey key, TItem item)
    {
        var otherFeatureStateContainerRecord = ConstructDeepClone();

        otherFeatureStateContainerRecord.FeatureStateMap[key] = item;

        return otherFeatureStateContainerRecord;
    }

    public int Count => FeatureStateKeys.Count;
    public ImmutableArray<TItem> Items => FeatureStateKeys
        .Select(key => FeatureStateMap[key])
        .ToImmutableArray();

    public TItem this[TKey key] => FeatureStateMap[key];
    public TItem this[int index] => FeatureStateMap[FeatureStateKeys[index]];
}