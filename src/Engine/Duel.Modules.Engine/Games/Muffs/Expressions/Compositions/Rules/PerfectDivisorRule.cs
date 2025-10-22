using Duel.Modules.Engine.Games.Muffs.Expressions.AST;

namespace Duel.Modules.Engine.Games.Muffs.Expressions.Compositions.Rules;

public sealed class PerfectDivisorRule : ICompositionRule
{
    public bool Predicate(Composition composition)
    {
        return composition.Type is Expression.Operator.Divide && composition.Left % composition.Right is 0;
    }
}