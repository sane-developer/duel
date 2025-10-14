using Duel.Modules.Engine.Games.Muffs.Expressions.Contexts;
using Duel.Modules.Engine.Games.Muffs.Expressions.Symbols;

namespace Duel.Modules.Engine.Games.Muffs.Expressions;

public sealed class ExpressionGenerator(ExpressionContext context)
{
    public Expression Generate()
    {
        var budget = GetRandomNumber(context.Operators.Start.Value, context.Operators.End.Value);

        var depth = GetRandomNumber(context.Depth.Start.Value, context.Depth.End.Value);

        return GetExpression(budget, depth);
    }

    private Expression GetExpression(int budget, int depth)
    {
        if (budget is 0 || depth is 0)
        {
            var value = GetRandomNumber(context.Constant.Start.Value, context.Constant.End.Value);

            return Constant.From(value);
        }

        var type = GetRandomOperatorType();

        var lb = GetLeftBudget(budget - 1);

        var rb = GetRightBudget(budget - 1, lb);

        var lhs = GetExpression(lb, depth - 1);

        if (type is Expression.Type.Divide)
        {
            var dividend = ExpressionEvaluator.Evaluate(lhs);

            var divisor = context.Divisors.GetRandomDivisor(context.Rng, dividend);

            var symbol = GetExpressionWithSpecificResult(divisor, rb, depth - 1);

            return Binary.From(type, lhs, symbol);
        }

        if (type is Expression.Type.Power)
        {
            var exponent = GetRandomExponent();

            var symbol = GetExpressionWithSpecificResult(exponent, rb, depth - 1);

            return Binary.From(type, lhs, symbol);
        }

        var rhs = GetExpression(rb, depth - 1);

        if (type is Expression.Type.Factorial or Expression.Type.SquareRoot)
        {
            return Absolute.From(rhs);
        }

        return Binary.From(type, lhs, rhs);
    }

    private Expression GetExpressionWithSpecificResult(int result, int budget, int depth)
    {
        if (budget is 0 || depth is 0)
        {
            return Constant.From(result);
        }

        var composition = context.Vault.GetRandomComposition(context.Rng, result);

        var lb = GetLeftBudget(budget - 1);
        
        var rb = GetRightBudget(budget - 1, lb);
        
        var lhs = GetExpressionWithSpecificResult(composition.Left, lb, depth - 1);
        
        var rhs = GetExpressionWithSpecificResult(composition.Right, rb, depth - 1);
        
        return Binary.From(composition.Type, lhs, rhs);
    }

    private Expression.Type GetRandomOperatorType()
    {
        var index = GetRandomNumber(0, context.Operations.Length - 1);
        
        return context.Operations[index];
    }

    private int GetRandomExponent()
    {
        return GetRandomNumber(context.Exponent.Start.Value, context.Exponent.End.Value);
    }

    private int GetRandomNumber(int minimum, int maximum)
    {
        return context.Rng.Next(minimum, maximum + 1);
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