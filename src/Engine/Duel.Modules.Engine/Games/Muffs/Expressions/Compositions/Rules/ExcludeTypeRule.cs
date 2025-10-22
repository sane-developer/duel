using Duel.Modules.Engine.Games.Muffs.Expressions.AST;

namespace Duel.Modules.Engine.Games.Muffs.Expressions.Compositions.Rules;

public sealed class ExcludeTypeRule(Expression.Operator type) : ICompositionRule
{
    public bool Predicate(Composition composition)
    {
        return composition.Type != type;
    }
}