namespace Duel.Modules.Engine.Games.Muffs.Expressions.Compositions.Filters;

public sealed class SmallerOrEqualResultFilter(int value) : ICompositionFilter
{
    public bool Predicate(Composition composition)
    {
        return composition.Result <= value;
    }
}