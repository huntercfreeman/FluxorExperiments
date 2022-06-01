namespace FluxorExperiments.ClassLibrary.FeatureStateContainer;

public static class Actions
{
    public record WithAddAction<TKey, TItem>(TKey Key, TItem Item);
    public record WithInsertAction<TKey, TItem>(TKey Key, TItem Item, int Index);
    public record WithRemoveAction<TKey>(TKey Key);
    public record WithReplaceAction<TKey, TItem>(TKey Key, TItem Item);
}