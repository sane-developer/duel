using Duel.Modules.Engine.Games.Muffs.Expressions.Symbols;

namespace Duel.Modules.Engine.Games.Muffs.Expressions.Settings;

internal static class ExpressionSettingsRegistry
{
    public static readonly ExpressionSettings Easy = ExpressionSettingsBuilder.New()
        .WithDepth(minimum: 1, maximum: 10)
        .WithBudget(minimum: 1, maximum: 10)
        .WithConstant(minimum: 1, maximum: 100)
        .WithExponent(minimum: 1, maximum: 10)
        .WithOperator(Expression.Operator.Add)
        .WithOperator(Expression.Operator.Subtract)
        .WithOperator(Expression.Operator.Multiply)
        .Build();

    public static readonly ExpressionSettings Medium = ExpressionSettingsBuilder.From(Easy)
        .WithOperator(Expression.Operator.Divide)
        .WithOperator(Expression.Operator.Power)
        .WithOperator(Expression.Operator.SquareRoot)
        .Build();

    public static readonly ExpressionSettings Hard = ExpressionSettingsBuilder.From(Medium)
        .WithOperator(Expression.Operator.Negate)
        .WithOperator(Expression.Operator.Absolute)
        .WithOperator(Expression.Operator.Factorial)
        .Build();
}