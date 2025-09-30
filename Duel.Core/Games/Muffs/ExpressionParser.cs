namespace Duel.Core.Games.Muffs;

public static class ExpressionParser
{
    public static Expression Parse(string expression)
    {
        return Expression.From(Operator.Null, int.MaxValue, int.MaxValue);
    }
}