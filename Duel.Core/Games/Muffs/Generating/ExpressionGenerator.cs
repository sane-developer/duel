using Duel.Core.Games.Muffs.AST;
using Duel.Core.Games.Muffs.Evaluating;

namespace Duel.Core.Games.Muffs.Generating;

public sealed class ExpressionGenerator(IExpressionStrategy strategy)
{
    public Expression Generate()
    {
        return GetNode(depth: 1);
    }

    private Expression GetNode(int depth)
    {
        if (strategy.IsReady(depth))
        {
            var value = strategy.GetConstant();

            return Constant.From(value);
        }

        var code = strategy.GetOperatorCode();

        var lhs = GetNode(depth + 1);

        if (code is Operator.Code.Divide)
        {
            return GetDivisionNode(lhs);
        }

        if (code is Operator.Code.Power)
        {
            return GetPowerNode(lhs);
        }

        var rhs = GetNode(depth);
        
        return Expression.From(code, lhs, rhs);
    }

    private Division GetDivisionNode(Expression lhs)
    {
        var evaluator = new ExpressionEvaluator(lhs);

        var dividend = evaluator.Evaluate();
        
        var divisor = strategy.GetDivisor(dividend);
        
        var rhs = Constant.From(divisor);
        
        return Division.From(lhs, rhs);
    }

    private Power GetPowerNode(Expression lhs)
    {
        var exponent = strategy.GetExponent();
        
        var rhs = Constant.From(exponent);
        
        return Power.From(lhs, rhs);
    }
}