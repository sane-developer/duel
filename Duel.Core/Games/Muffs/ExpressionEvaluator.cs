namespace Duel.Core.Games.Muffs;

public static class ExpressionEvaluator
{
    public static int Evaluate(Expression expression)
    {
        if (!expression.Root.IsOperator)
        {
            return expression.Root.Value;
        }

        var lhs = Evaluate(expression.Left!);

        var rhs = Evaluate(expression.Right!);
                
        return expression.Root.Operator.Callback(lhs, rhs);
    }
}