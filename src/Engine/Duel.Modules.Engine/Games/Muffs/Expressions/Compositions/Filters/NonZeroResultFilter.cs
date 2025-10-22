namespace Duel.Modules.Engine.Games.Muffs.Expressions.Compositions.Filters;

public sealed class NonZeroResultFilter : ICompositionFilter
{
    public bool Predicate(Composition composition)
    {
        return composition.Result is not 0;
    }
}