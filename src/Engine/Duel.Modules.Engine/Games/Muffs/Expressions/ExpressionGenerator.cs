using Duel.Modules.Engine.Games.Muffs.Expressions.Contexts;
using Duel.Modules.Engine.Games.Muffs.Expressions.Symbols;

namespace Duel.Modules.Engine.Games.Muffs.Expressions;

public sealed class ExpressionGenerator(Random rng, ExpressionContext context)
{
    public Expression Generate()
    {
        var budget = context.GetBudget(rng);

        var depth = context.GetDepth(rng);

        return GetExpression(budget, depth);
    }

    private Expression GetExpression(int budget, int depth)
    {
        if (budget is 0 || depth is 0)
        {
            var value = context.GetConstant(rng);

            return context.Constants.Get(value);
        }

        var type = context.GetOperatorType(rng);

        var lb = GetLeftBudget(budget - 1);

        var rb = GetRightBudget(budget - 1, lb);

        var lhs = GetExpression(lb, depth - 1);

        if (type is Expression.Operator.Divide)
        {
            var dividend = ExpressionEvaluator.Evaluate(lhs);

            var divisor = context.GetDivisor(rng, dividend);

            var symbol = GetExpression(divisor, rb, depth - 1);

            return Binary.From(type, lhs, symbol);
        }

        if (type is Expression.Operator.Power)
        {
            var exponent = context.GetExponent(rng);

            var symbol = GetExpression(exponent, rb, depth - 1);

            return Binary.From(type, lhs, symbol);
        }

        var rhs = GetExpression(rb, depth - 1);

        if (type is Expression.Operator.Factorial or Expression.Operator.SquareRoot)
        {
            return Unary.From(type, rhs);
        }

        return Binary.From(type, lhs, rhs);
    }

    private Expression GetExpression(int result, int budget, int depth)
    {
        if (budget is 0 || depth is 0)
        {
            return context.GetConstant(result);
        }

        var lb = GetLeftBudget(budget - 1);
        
        var rb = GetRightBudget(budget - 1, lb);
        
        var composition = context.GetComposition(rng, result);
        
        var lhs = GetExpression(composition.Left, lb, depth - 1);
        
        var rhs = GetExpression(composition.Right, rb, depth - 1);
        
        return Binary.From(composition.Type, lhs, rhs);
    }

    private static int GetLeftBudget(int totalBudget)
    {
        return (int) Math.Ceiling(totalBudget / 2d);
    }

    private static int GetRightBudget(int totalBudget, int leftBudget)
    {
        return totalBudget - leftBudget;
    }
}