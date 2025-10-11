using Duel.Modules.Engine.Games.Muffs.Expressions.Evaluators;
using Duel.Modules.Engine.Games.Muffs.Expressions.Symbols;

namespace Duel.Modules.Engine.Games.Muffs.Expressions.Generators;

public sealed class BalancedExpressionGenerator(IExpressionContext context) : IExpressionGenerator
{
    public Expression Generate()
    {
        var budget = GetRandomNumber(context.Operators.Minimum, context.Operators.Maximum);

        var depth = GetRandomNumber(context.Depth.Minimum, context.Depth.Maximum);

        return GetExpression(budget, depth);
    }

    private Expression GetExpression(int budget, int depth)
    {
        if (budget is 0 || depth is 0)
        {
            var value = GetRandomNumber(context.Constant.Minimum, context.Constant.Maximum);

            return Constant.From(value);
        }

        var type = GetRandomOperatorType();

        var lb = GetLeftBudget(budget - 1);

        var rb = GetRightBudget(budget - 1, lb);

        var lhs = GetExpression(lb, depth - 1);

        if (type is ExpressionType.Divide)
        {
            var dividend = ExpressionEvaluator.Evaluate(lhs);

            var divisor = GetRandomDivisor(dividend);

            var symbol = GetExpressionWithSpecificResult(divisor, rb, depth - 1);

            return Binary.From(type, lhs, symbol);
        }

        if (type is ExpressionType.Power)
        {
            var exponent = GetRandomExponent();

            var symbol = GetExpressionWithSpecificResult(exponent, rb, depth - 1);

            return Binary.From(type, lhs, symbol);
        }

        var rhs = GetExpression(rb, depth - 1);

        if (type is ExpressionType.Factorial or ExpressionType.SquareRoot)
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

        var operations = context.Vault.GetAvailableOperations(result);
        
        if (operations.Count is 0)
        {
            return Constant.From(result);
        }

        var index = context.Rng.Next(operations.Count);
        
        var type = operations[index];
        
        var operands = context.Vault.GetRandomOperands(type, result);
        
        if (operands is null)
        {
            return Constant.From(result);
        }

        var lb = GetLeftBudget(budget - 1);
        
        var rb = GetRightBudget(budget - 1, lb);
        
        var lhs = GetExpressionWithSpecificResult(operands.Value.Left, lb, depth - 1);
        
        var rhs = GetExpressionWithSpecificResult(operands.Value.Right, rb, depth - 1);
        
        return Binary.From(type, lhs, rhs);
    }

    private ExpressionType GetRandomOperatorType()
    {
        const int lowestIndex = (int) ExpressionType.Add;

        const int highestIndex = (int) ExpressionType.Factorial;
        
        return (ExpressionType) GetRandomNumber(lowestIndex, highestIndex);
    }

    private int GetRandomExponent()
    {
        return GetRandomNumber(context.Exponent.Minimum, context.Exponent.Maximum);
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

    private int GetRandomDivisor(int dividend)
    {
        var cursor = 0;

        var abs = Math.Abs(dividend);
        
        var limit = (int) Math.Sqrt(abs);

        const int complementaryPairsMultiplier = 2;

        Span<int> divisors = stackalloc int[limit * complementaryPairsMultiplier];
        
        for (var divisor = 1; divisor <= limit; divisor++)
        {
            if (abs % divisor != 0)
            {
                continue;
            }

            divisors[cursor++] = divisor;
            
            var complementary = abs / divisor;
            
            if (complementary != divisor)
            {
                divisors[cursor++] = complementary;
            }
        }

        var index = context.Rng.Next(cursor);

        return divisors[index];
    }
}