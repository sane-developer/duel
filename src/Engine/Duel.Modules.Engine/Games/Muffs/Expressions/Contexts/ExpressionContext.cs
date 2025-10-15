using Duel.Modules.Engine.Games.Muffs.Expressions.Symbols;
using Duel.Modules.Engine.Games.Muffs.Expressions.Vaults;

namespace Duel.Modules.Engine.Games.Muffs.Expressions.Contexts;

public sealed class ExpressionContext(ExpressionSettings settings)
{
    public required DivisorVault Divisors { get; init; }

    public required ConstantVault Constants { get; init; }

    public required CompositionVault Compositions { get; init; }

    public int GetRandomDepth(Random rng)
    {
        return GetRandomNumber(rng, settings.Depth.Start.Value, settings.Depth.End.Value);
    }

    public int GetRandomBudget(Random rng)
    {
        return GetRandomNumber(rng, settings.Budget.Start.Value, settings.Budget.End.Value);
    }

    public int GetRandomConstant(Random rng)
    {
        return GetRandomNumber(rng, settings.Constant.Start.Value, settings.Constant.End.Value);
    }

    public int GetRandomExponent(Random rng)
    {
        return GetRandomNumber(rng, settings.Exponent.Start.Value, settings.Exponent.End.Value);
    }

    public Expression.Operator GetRandomOperator(Random rng)
    {
        var index = GetRandomIndex(rng, settings.Operators.Length);

        return settings.Operators[index];
    }

    private static int GetRandomNumber(Random rng, int minimum, int maximum)
    {
        return rng.Next(minimum, maximum + 1);
    }

    private static int GetRandomIndex(Random rng, int length)
    {
        return rng.Next(length);
    }
}

public static class ExpressionContextRegistry
{
    public static readonly ExpressionContext Easy = ExpressionContextFactory.Create(
        ExpressionSettingsRegistry.Easy
    );

    public static readonly ExpressionContext Medium = ExpressionContextFactory.Create(
        ExpressionSettingsRegistry.Medium
    );

    public static readonly ExpressionContext Hard = ExpressionContextFactory.Create(
        ExpressionSettingsRegistry.Hard
    );
}

file static class ExpressionContextFactory
{
    public static ExpressionContext Create(ExpressionSettings settings)
    {
        return new ExpressionContext(settings)
        {
            Divisors = DivisorVault.Create(settings.Constant),
            Constants = ConstantVault.Create(settings.Constant),
            Compositions = CompositionVault.Create(settings.Constant, settings.Operators)
        };
    }
}