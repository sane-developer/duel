using Duel.Modules.Engine.Games.Muffs.Expressions.Symbols;
using Duel.Modules.Engine.Games.Muffs.Expressions.Vaults;

namespace Duel.Modules.Engine.Games.Muffs.Expressions.Contexts;

public sealed class ExpressionContext(ExpressionSettings settings)
{
    public Range Depth => settings.Depth;

    public Range Constant => settings.Constant;

    public Range Exponent => settings.Exponent;

    public Range Budget => settings.Budget;

    public Expression.Operator[] Operators => settings.Operators;

    public required DivisorVault Divisors { get; init; }

    public required CompositionVault Compositions { get; init; }
}

public static class ExpressionContextFactory
{
    public static ExpressionContext Create(ExpressionSettings settings)
    {
        var divisors = DivisorVault.Create(settings.Constant);

        var compositions = CompositionVault.Create(settings.Constant, settings.Operators);

        return new ExpressionContext(settings)
        {
            Divisors = divisors, Compositions = compositions
        };
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