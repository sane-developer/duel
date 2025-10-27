namespace Duel.Modules.Engine.Games.Muffs.Generation;

public static class DifficultyPresets
{
    public static readonly GeneratorSettings Easy = SettingsBuilder.New()
        .WithDepth(1..3)
        .WithLength(1..5)
        .WithAdditions(weight: 1.0, range: 1..10)
        .WithSubtractions(weight: 1.0, range: 1..10)
        .WithMultiplications(weight: 1.0, range: 1..10)
        .Build();

    public static readonly GeneratorSettings Medium = SettingsBuilder.New()
        .WithDepth(2..4)
        .WithLength(5..8)
        .WithAdditions(weight: 1.0, range: new IntegerRange(-10, 20))
        .WithSubtractions(weight: 1.0, range: new IntegerRange(-10, 20))
        .WithMultiplications(weight: 1.0, range: 1..12)
        .WithDivisions(weight: 1.0, range: 1..50)
        .WithModulos(weight: 0.5, range: 1..20)
        .WithPowers(weight: 0.5, range: 1..5)
        .Build();

    public static readonly GeneratorSettings Hard = SettingsBuilder.New()
        .WithDepth(3..5)
        .WithLength(8..10)
        .WithAdditions(weight: 1.0, range: new IntegerRange(-20, 30))
        .WithSubtractions(weight: 1.0, range: new IntegerRange(-20, 30))
        .WithMultiplications(weight: 1.0, range: 1..15)
        .WithDivisions(weight: 1.0, range: 1..100)
        .WithModulos(weight: 0.8, range: 1..30)
        .WithPowers(weight: 0.5, range: 1..5)
        .WithNegations(weight: 0.8, range: new IntegerRange(-50, 50))
        .WithAbsoluteValues(weight: 0.8, range: new IntegerRange(-50, 50))
        .WithFactorials(weight: 0.3, range: 0..5)
        .WithSquareRoots(weight: 0.3, range: 0..100)
        .Build();
}

file sealed class SettingsBuilder(GeneratorSettings settings)
{
    public static SettingsBuilder New()
    {
        var settings = new GeneratorSettings();

        return new SettingsBuilder(settings);
    }

    public SettingsBuilder WithDepth(Range depth)
    {
        settings = settings with { Depth = depth };
        
        return this;
    }

    public SettingsBuilder WithLength(Range length)
    {
        settings = settings with { Length = length };
        
        return this;
    }

    public SettingsBuilder WithAdditions(double weight, IntegerRange range)
    {
        settings = settings with { Addition = new OperatorSettings(weight, range) };
        
        return this;
    }

    public SettingsBuilder WithSubtractions(double weight, IntegerRange range)
    {
        settings = settings with { Subtraction = new OperatorSettings(weight, range) };
        
        return this;
    }

    public SettingsBuilder WithMultiplications(double weight, IntegerRange range)
    {
        settings = settings with { Multiplication = new OperatorSettings(weight, range) };
        
        return this;
    }

    public SettingsBuilder WithDivisions(double weight, IntegerRange range)
    {
        settings = settings with { Division = new OperatorSettings(weight, range) };
        
        return this;
    }

    public SettingsBuilder WithModulos(double weight, IntegerRange range)
    {
        settings = settings with { Modulo = new OperatorSettings(weight, range) };
        
        return this;
    }

    public SettingsBuilder WithPowers(double weight, IntegerRange range)
    {
        settings = settings with { Power = new OperatorSettings(weight, range) };
        
        return this;
    }

    public SettingsBuilder WithNegations(double weight, IntegerRange range)
    {
        settings = settings with { Negation = new OperatorSettings(weight, range) };
        
        return this;
    }

    public SettingsBuilder WithAbsoluteValues(double weight, IntegerRange range)
    {
        settings = settings with { AbsoluteValue = new OperatorSettings(weight, range) };
        
        return this;
    }

    public SettingsBuilder WithFactorials(double weight, IntegerRange range)
    {
        settings = settings with { Factorial = new OperatorSettings(weight, range) };
        
        return this;
    }

    public SettingsBuilder WithSquareRoots(double weight, IntegerRange range)
    {
        settings = settings with { SquareRoot = new OperatorSettings(weight, range) };
        
        return this;
    }

    public GeneratorSettings Build()
    {
        return settings;
    }
}

