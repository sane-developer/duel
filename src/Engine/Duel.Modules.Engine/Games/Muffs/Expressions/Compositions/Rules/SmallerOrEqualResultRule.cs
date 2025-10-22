namespace Duel.Modules.Engine.Games.Muffs.Expressions.Compositions.Rules;

public sealed class SmallerOrEqualResultRule(int value) : ICompositionRule
{
    public bool Predicate(Composition composition)
    {
        return composition.Result <= value;
    }
}