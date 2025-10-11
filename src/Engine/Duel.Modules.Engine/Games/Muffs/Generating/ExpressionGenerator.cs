using Duel.Modules.Engine.Games.Muffs.AST;
using Duel.Shared.Ranges;

namespace Duel.Modules.Engine.Games.Muffs.Generating;

public interface IExpressionContext
{
    /// <summary>
    ///     The random number generator.
    /// </summary>
    Random Rng { get; }

    /// <summary>
    ///     The range of values representing the possible depth of the entire expression.
    /// </summary>
    Range<int> Depth { get; }

    /// <summary>
    ///     The range of values representing the possible constant values.
    /// </summary>
    Range<int> Constant { get; }

    /// <summary>
    ///     The range of values representing the possible count of operators in the entire expression.
    /// </summary>
    Range<int> Operators { get; }
}

public interface IExpressionGenerator
{
    Expression Generate();
}

public sealed class RandomExpressionGenerator(IExpressionContext context) : IExpressionGenerator
{
    public Expression Generate()
    {
        var budget = GetRandomNumber(context.Operators.Minimum, context.Operators.Maximum);

        return GetExpression(budget);
    }

    private Expression GetExpression(int budget)
    {
        if (budget == 0)
        {
            return GetRandomConstant();
        }

        var type = GetRandomOperatorType();

        var lb = GetLeftBudget(budget - 1);

        var rb = GetRightBudget(budget - 1, lb);

        var lhs = GetExpression(lb);

        var rhs = GetExpression(rb);

        return Binary.From(type, lhs, rhs);
    }

    private Constant GetRandomConstant()
    {
        var value = GetRandomNumber(context.Constant.Minimum, context.Constant.Maximum);

        return Constant.From(value);
    }

    private ExpressionType GetRandomOperatorType()
    {
        const int lowestIndex = (int) ExpressionType.Add;
        
        const int highestIndex = (int) ExpressionType.Factorial;

        return (ExpressionType) GetRandomNumber(lowestIndex, highestIndex);
    }

    private int GetRandomNumber(int minimum, int maximum)
    {
        return context.Rng.Next(minimum, maximum + 1);
    }

    private static int GetLeftBudget(int remainingOperations)
    {
        return (int) Math.Ceiling(remainingOperations / 2d);
    }

    private static int GetRightBudget(int remainingOperations, int leftBudget)
    {
        return remainingOperations - leftBudget;
    }
}