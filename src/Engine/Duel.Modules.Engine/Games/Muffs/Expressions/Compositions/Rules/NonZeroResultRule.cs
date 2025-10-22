namespace Duel.Modules.Engine.Games.Muffs.Expressions.Compositions.Rules;

public sealed class NonZeroResultRule : ICompositionRule
{
    public bool Predicate(Composition composition)
    {
        return composition.Result is not 0;
    }
}