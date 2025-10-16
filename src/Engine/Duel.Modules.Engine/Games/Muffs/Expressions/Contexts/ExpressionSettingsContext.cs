using Duel.Modules.Engine.Games.Muffs.Expressions.Symbols;

namespace Duel.Modules.Engine.Games.Muffs.Expressions.Contexts;

public record struct ExpressionSettings
{
    public Range Depth { get; set; }

    public Range Budget { get; set; }

    public Range Constant { get; set; }

    public Range Exponent { get; set; }

    public Expression.Operator[] Operators { get; set; }

    public ExpressionSettings()
    {
        Operators = [];
    }
}

public sealed class ExpressionSettingsContext(ExpressionSettings settings)
{
    public int GetDepth(Random rng)
    {
        return GetRandomNumber(rng, settings.Depth.Start.Value, settings.Depth.End.Value);
    }

    public int GetBudget(Random rng)
    {
        return GetRandomNumber(rng, settings.Budget.Start.Value, settings.Budget.End.Value);
    }

    public int GetConstant(Random rng)
    {
        return GetRandomNumber(rng, settings.Constant.Start.Value, settings.Constant.End.Value);
    }

    public int GetExponent(Random rng)
    {
        return GetRandomNumber(rng, settings.Exponent.Start.Value, settings.Exponent.End.Value);
    }

    public Expression.Operator GetOperatorType(Random rng)
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

public static class ExpressionSettingsContextFactory
{
    public static ExpressionSettingsContext Easy()
    {
        return From(ExpressionSettingsRegistry.Easy);
    }

    public static ExpressionSettingsContext Medium()
    {
        return From(ExpressionSettingsRegistry.Medium);
    }

    public static ExpressionSettingsContext Hard()
    {
        return From(ExpressionSettingsRegistry.Hard);
    }

    private static ExpressionSettingsContext From(ExpressionSettings settings)
    {
        return new ExpressionSettingsContext(settings);
    }
}

file static class ExpressionSettingsRegistry
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

file sealed class ExpressionSettingsBuilder(ExpressionSettings settings)
{
    public static ExpressionSettingsBuilder New()
    {
        var settings = new ExpressionSettings();
        
        return new ExpressionSettingsBuilder(settings);
    }

    public static ExpressionSettingsBuilder From(ExpressionSettings settings)
    {
        return new ExpressionSettingsBuilder(settings);
    }

    public ExpressionSettingsBuilder WithOperator(Expression.Operator @operator)
    {
        settings.Operators = [.. settings.Operators.Concat([@operator]).Distinct()];

        return this;
    }

    public ExpressionSettingsBuilder WithDepth(int minimum, int maximum)
    {
        settings.Depth = new Range(minimum, maximum);

        return this;
    }

    public ExpressionSettingsBuilder WithBudget(int minimum, int maximum)
    {
        settings.Budget = new Range(minimum, maximum);

        return this;
    }

    public ExpressionSettingsBuilder WithConstant(int minimum, int maximum)
    {
        settings.Constant = new Range(minimum, maximum);

        return this;
    }

    public ExpressionSettingsBuilder WithExponent(int minimum, int maximum)
    {
        settings.Exponent = new Range(minimum, maximum);

        return this;
    }

    public ExpressionSettings Build()
    {
        return settings;
    }
}