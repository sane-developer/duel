namespace Duel.Modules.Engine.Games.Muffs;

public static class ExpressionSettingsRegistry
{
    public static readonly ExpressionSettings Easy = ExpressionSettingsBuilder.New()
        .WithDepth(1..3)
        .WithLength(1..5)
        .WithAdditions(weight: 1.0, operandRange: 1..10, resultRange: 0..50)
        .WithSubtractions(weight: 1.0, operandRange: 1..10, resultRange: 0..50)
        .WithMultiplications(weight: 1.0, operandRange: 1..10, resultRange: 0..100)
        .Build();

    public static readonly ExpressionSettings Medium = ExpressionSettingsBuilder.New()
        .WithDepth(2..4)
        .WithLength(1..8)
        .WithAdditions(weight: 1.0, operandRange: new NumericRange(-10, 20), resultRange: new NumericRange(-50, 100))
        .WithSubtractions(weight: 1.0, operandRange: new NumericRange(-10, 20), resultRange: new NumericRange(-50, 100))
        .WithMultiplications(weight: 1.0, operandRange: 1..12, resultRange: 0..150)
        .WithDivisions(weight: 1.0, operandRange: 1..50, resultRange: 0..50)
        .WithModulos(weight: 0.5, operandRange: 1..20, resultRange: 0..20)
        .WithPowers(weight: 0.5, operandRange: 1..5, resultRange: 0..125)
        .Build();

    public static readonly ExpressionSettings Hard = ExpressionSettingsBuilder.New()
        .WithDepth(3..5)
        .WithLength(1..10)
        .WithAdditions(weight: 1.0, operandRange: new NumericRange(-20, 30), resultRange: new NumericRange(-150, 150))
        .WithSubtractions(weight: 1.0, operandRange: new NumericRange(-20, 30), resultRange: new NumericRange(-150, 150))
        .WithMultiplications(weight: 1.0, operandRange: 1..15, resultRange: new NumericRange(-200, 200))
        .WithDivisions(weight: 1.0, operandRange: 1..100, resultRange: new NumericRange(-100, 100))
        .WithModulos(weight: 0.8, operandRange: 1..30, resultRange: 0..30)
        .WithPowers(weight: 0.5, operandRange: 1..5, resultRange: 0..125)
        .WithNegations(weight: 0.8, operandRange: new NumericRange(-50, 50), resultRange: new NumericRange(-150, 150))
        .WithAbsoluteValues(weight: 0.8, operandRange: new NumericRange(-50, 50), resultRange: 0..150)
        .WithFactorials(weight: 0.3, operandRange: 0..5, resultRange: 1..120)
        .WithSquareRoots(weight: 0.3, operandRange: 0..100, resultRange: 0..10)
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

    public ExpressionSettingsBuilder WithLength(Range length)
    {
        settings = settings with { Length = length };
        return this;
    }

    public ExpressionSettingsBuilder WithAdditions(double weight, NumericRange operandRange, NumericRange? resultRange = null)
    {
        settings = settings with { Addition = new OperatorSettings(weight, operandRange, resultRange) };
        return this;
    }

    public ExpressionSettingsBuilder WithSubtractions(double weight, NumericRange operandRange, NumericRange? resultRange = null)
    {
        settings = settings with { Subtraction = new OperatorSettings(weight, operandRange, resultRange) };
        return this;
    }

    public ExpressionSettingsBuilder WithMultiplications(double weight, NumericRange operandRange, NumericRange? resultRange = null)
    {
        settings = settings with { Multiplication = new OperatorSettings(weight, operandRange, resultRange) };
        return this;
    }

    public ExpressionSettingsBuilder WithDivisions(double weight, NumericRange operandRange, NumericRange? resultRange = null)
    {
        settings = settings with { Division = new OperatorSettings(weight, operandRange, resultRange) };
        return this;
    }

    public ExpressionSettingsBuilder WithModulos(double weight, NumericRange operandRange, NumericRange? resultRange = null)
    {
        settings = settings with { Modulo = new OperatorSettings(weight, operandRange, resultRange) };
        return this;
    }

    public ExpressionSettingsBuilder WithPowers(double weight, NumericRange operandRange, NumericRange? resultRange = null)
    {
        settings = settings with { Power = new OperatorSettings(weight, operandRange, resultRange) };
        return this;
    }

    public ExpressionSettingsBuilder WithNegations(double weight, NumericRange operandRange, NumericRange? resultRange = null)
    {
        settings = settings with { Negation = new OperatorSettings(weight, operandRange, resultRange) };
        return this;
    }

    public ExpressionSettingsBuilder WithAbsoluteValues(double weight, NumericRange operandRange, NumericRange? resultRange = null)
    {
        settings = settings with { AbsoluteValue = new OperatorSettings(weight, operandRange, resultRange) };
        return this;
    }

    public ExpressionSettingsBuilder WithFactorials(double weight, NumericRange operandRange, NumericRange? resultRange = null)
    {
        settings = settings with { Factorial = new OperatorSettings(weight, operandRange, resultRange) };
        return this;
    }

    public ExpressionSettingsBuilder WithSquareRoots(double weight, NumericRange operandRange, NumericRange? resultRange = null)
    {
        settings = settings with { SquareRoot = new OperatorSettings(weight, operandRange, resultRange) };
        return this;
    }

    public ExpressionSettings Build()
    {
        return settings;
    }
}