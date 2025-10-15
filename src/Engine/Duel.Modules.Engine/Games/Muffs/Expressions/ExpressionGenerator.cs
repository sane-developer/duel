using Duel.Modules.Engine.Games.Muffs.Expressions.Contexts;
using Duel.Modules.Engine.Games.Muffs.Expressions.Symbols;

namespace Duel.Modules.Engine.Games.Muffs.Expressions;

public sealed class ExpressionGenerator(Random rng,ExpressionContext context)
{
    public Expression Generate()
    {
        var budget = context.GetRandomBudget(rng);

        var depth = context.GetRandomDepth(rng);

        return GetExpression(budget, depth);
    }

    private Expression GetExpression(int budget, int depth)
    {
        if (budget is 0 || depth is 0)
        {
            var value = context.GetRandomConstant(rng);

            return context.Constants.Get(value);
        }

        var type = context.GetRandomOperator(rng);

        var lb = GetLeftBudget(budget - 1);

        var rb = GetRightBudget(budget - 1, lb);

        var lhs = GetExpression(lb, depth - 1);

        if (type is Expression.Operator.Divide)
        {
            var dividend = ExpressionEvaluator.Evaluate(lhs);

            var divisor = context.Divisors.GetRandom(dividend, rng);

            var symbol = GetExpression(divisor, rb, depth - 1);

            return Binary.From(type, lhs, symbol);
        }

        if (type is Expression.Operator.Power)
        {
            var exponent = context.GetRandomExponent(rng);

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
            return context.Constants.Get(result);
        }

        var composition = context.Compositions.GetRandom(result, rng);

        var lb = GetLeftBudget(budget - 1);
        
        var rb = GetRightBudget(budget - 1, lb);
        
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