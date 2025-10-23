using Duel.Modules.Engine.Games.Muffs.AST.Literals;

namespace Duel.Modules.Engine.Games.Muffs;

public sealed class ExpressionContext(ExpressionSettings settings)
{
    private readonly MuffsCache _cache = new(settings);

    public Number GetNumber(int value)
    {
        return _cache.GetNumber(value);
    }

    public int[] GetDivisors(int dividend)
    {
        return _cache.GetDivisors(dividend);
    }

    public ExpressionComposition[] GetCompositions(int value)
    {
        return _cache.GetCompositions(value);
    }
}