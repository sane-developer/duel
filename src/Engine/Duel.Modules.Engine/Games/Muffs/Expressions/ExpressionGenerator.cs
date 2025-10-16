using Duel.Modules.Engine.Games.Muffs.Expressions.Contexts;
using Duel.Modules.Engine.Games.Muffs.Expressions.Symbols;

namespace Duel.Modules.Engine.Games.Muffs.Expressions;

public sealed class ExpressionGenerator(ExpressionCacheContext cacheContext, ExpressionSettingsContext settingsContext)
{
    public Expression Generate(Random rng)
    {
        var budget = settingsContext.GetBudget(rng);

        var depth = settingsContext.GetDepth(rng);

        return GetExpression(rng, budget, depth);
    }

    private Expression GetExpression(Random rng, int budget, int depth)
    {
        if (budget is 0 || depth is 0)
        {
            var value = settingsContext.GetConstant(rng);

            return cacheContext.GetConstant(value);
        }

        var operatorType = settingsContext.GetOperatorType(rng);

        var leftBudget = GetLeftBudget(budget - 1);

        var rightBudget = GetRightBudget(budget - 1, leftBudget);

        var lhs = GetExpression(rng, leftBudget, depth - 1);

        if (operatorType is Expression.Operator.Divide)
        {
            var dividend = ExpressionEvaluator.Evaluate(lhs);

            var divisor = cacheContext.GetDivisor(rng, dividend);

            var division = GetExpression(rng, divisor, rightBudget, depth - 1);

            return Binary.From(operatorType, lhs, division);
        }

        if (operatorType is Expression.Operator.Power)
        {
            var exponent = settingsContext.GetExponent(rng);

            var power = GetExpression(rng, exponent, rightBudget, depth - 1);

            return Binary.From(operatorType, lhs, power);
        }

        var rhs = GetExpression(rng, rightBudget, depth - 1);

        if (operatorType is Expression.Operator.Factorial or Expression.Operator.SquareRoot)
        {
            return Unary.From(operatorType, rhs);
        }

        return Binary.From(operatorType, lhs, rhs);
    }

    private Expression GetExpression(Random rng, int result, int budget, int depth)
    {
        if (budget is 0 || depth is 0)
        {
            return cacheContext.GetConstant(result);
        }

        var composition = cacheContext.GetComposition(rng, result);

        var leftBudget = GetLeftBudget(budget - 1);
        
        var rightBudget = GetRightBudget(budget - 1, leftBudget);
        
        var lhs = GetExpression(rng, composition.Left, leftBudget, depth - 1);
        
        var rhs = GetExpression(rng, composition.Right, rightBudget, depth - 1);
        
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