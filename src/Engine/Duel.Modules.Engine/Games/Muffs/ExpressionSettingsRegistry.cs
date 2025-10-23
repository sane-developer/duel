namespace Duel.Modules.Engine.Games.Muffs;

public static class ExpressionSettingsRegistry
{
    public static readonly ExpressionSettings Easy = ExpressionSettingsBuilder.New()
        .WithDepth(1..10)
        .WithBudget(1..10)
        .WithNumber(1..10)
        .WithAdditions(allow: true, weight: 1.0)
        .WithSubtractions(allow: true, weight: 1.0)
        .WithMultiplications(allow: true, weight: 1.0)
        .Build();

    public static readonly ExpressionSettings Medium = ExpressionSettingsBuilder.From(Easy)
        .WithDivisions(allow: true, weight: 1.0)
        .WithModulos(allow: true, weight: 1.0)
        .WithPowers(allow: true, weight: 1.0)
        .Build();

    public static readonly ExpressionSettings Hard = ExpressionSettingsBuilder.From(Medium)
        .WithAbsoluteValues(allow: true, weight: 1.0)
        .WithSquareRoots(allow: true, weight: 1.0)
        .WithFactorials(allow: true, weight: 1.0)
        .WithNegations(allow: true, weight: 1.0)
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

    public ExpressionSettingsBuilder WithDepth(Range depth)
    {
        settings = settings with { Depth = depth };

        return this;
    }

    public ExpressionSettingsBuilder WithBudget(Range budget)
    {
        settings = settings with { Budget = budget };

        return this;
    }

    public ExpressionSettingsBuilder WithNumber(Range number)
    {
        settings = settings with { Number = number };

        return this;
    }

    public ExpressionSettingsBuilder WithAdditions(bool allow, double weight)
    {
        settings = settings with { Addition = new OperatorSettings(allow ? weight : 0d) };

        return this;
    }

    public ExpressionSettingsBuilder WithSubtractions(bool allow, double weight)
    {
        settings = settings with { Subtraction = new OperatorSettings(allow ? weight : 0d) };

        return this;
    }

    public ExpressionSettingsBuilder WithMultiplications(bool allow, double weight)
    {
        settings = settings with { Multiplication = new OperatorSettings(allow ? weight : 0d) };

        return this;
    }

    public ExpressionSettingsBuilder WithDivisions(bool allow, double weight)
    {
        settings = settings with { Division = new OperatorSettings(allow ? weight : 0d) };

        return this;
    }

    public ExpressionSettingsBuilder WithModulos(bool allow, double weight)
    {
        settings = settings with { Modulo = new OperatorSettings(allow ? weight : 0d) };

        return this;
    }

    public ExpressionSettingsBuilder WithPowers(bool allow, double weight)
    {
        settings = settings with { Power = new OperatorSettings(allow ? weight : 0d) };

        return this;
    }

    public ExpressionSettingsBuilder WithNegations(bool allow, double weight)
    {
        settings = settings with { Negation = new OperatorSettings(allow ? weight : 0d) };

        return this;
    }

    public ExpressionSettingsBuilder WithAbsoluteValues(bool allow, double weight)
    {
        settings = settings with { AbsoluteValue = new OperatorSettings(allow ? weight : 0d) };

        return this;
    }

    public ExpressionSettingsBuilder WithFactorials(bool allow, double weight)
    {
        settings = settings with { Factorial = new OperatorSettings(allow ? weight : 0d) };

        return this;
    }

    public ExpressionSettingsBuilder WithSquareRoots(bool allow, double weight)
    {
        settings = settings with { SquareRoot = new OperatorSettings(allow ? weight : 0d) };

        return this;
    }

    public ExpressionSettings Build()
    {
        return settings;
    }
}