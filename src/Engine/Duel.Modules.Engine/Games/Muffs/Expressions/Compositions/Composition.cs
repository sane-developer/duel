using Duel.Modules.Engine.Games.Muffs.Expressions.AST;

namespace Duel.Modules.Engine.Games.Muffs.Expressions.Compositions;

public readonly record struct Composition(Expression.Operator Type, int Left, int Right, int Result)
{
    public static Composition From(Expression.Operator type, int left, int right, int result)
    {
        return new Composition(type, left, right, result);
    }
}