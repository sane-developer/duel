using Duel.Modules.Engine.Games.Muffs.Generation.Operators;

namespace Duel.Modules.Engine.Games.Muffs.Generation.Difficulties;

public static class DifficultyRegistry
{
    public static readonly Difficulty Easy = SettingsBuilder.New()
        .WithDepth(1..3)
        .WithLength(1..5)
        .WithAdditions(weight: 1.0, operands: 1..10)
        .WithSubtractions(weight: 1.0, operands: 1..10)
        .WithMultiplications(weight: 1.0, operands: 1..10)
        .Build();

    public static readonly Difficulty Medium = SettingsBuilder.New()
        .WithDepth(2..4)
        .WithLength(5..8)
        .WithAdditions(weight: 1.0, operands: -10..20)
        .WithSubtractions(weight: 1.0, operands: -10..20)
        .WithMultiplications(weight: 1.0, operands: 1..12)
        .WithDivisions(weight: 1.0, operands: 1..50)
        .WithModulos(weight: 0.5, operands: 1..20)
        .WithPowers(weight: 0.5, operands: 1..5)
        .Build();

    public static readonly Difficulty Hard = SettingsBuilder.New()
        .WithDepth(3..5)
        .WithLength(8..10)
        .WithAdditions(weight: 1.0, operands: -20..30)
        .WithSubtractions(weight: 1.0, operands: -20..30)
        .WithMultiplications(weight: 1.0, operands: 1..15)
        .WithDivisions(weight: 1.0, operands: 1..100)
        .WithModulos(weight: 0.8, operands: 1..30)
        .WithPowers(weight: 0.5, operands: 1..5)
        .WithNegations(weight: 0.8, operands: -50..50)
        .WithAbsoluteValues(weight: 0.8, operands: -50..0)
        .WithFactorials(weight: 0.3, operands: 0..5)
        .WithSquareRoots(weight: 0.3, operands: 0..100)
        .Build();
}

file sealed class SettingsBuilder(Difficulty settings)
{
    private readonly Dictionary<OperatorType, OperatorSettings> _operators = [];

    public static SettingsBuilder New()
    {
        var settings = new Difficulty();

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

    public SettingsBuilder WithAdditions(double weight, IntegerRange operands)
    {
        _operators[OperatorType.Addition] = new OperatorSettings(weight, operands);
        
        return this;
    }

    public SettingsBuilder WithSubtractions(double weight, IntegerRange operands)
    {
        _operators[OperatorType.Subtraction] = new OperatorSettings(weight, operands);
        
        return this;
    }

    public SettingsBuilder WithMultiplications(double weight, IntegerRange operands)
    {
        _operators[OperatorType.Multiplication] = new OperatorSettings(weight, operands);
        
        return this;
    }

    public SettingsBuilder WithDivisions(double weight, IntegerRange operands)
    {
        _operators[OperatorType.Division] = new OperatorSettings(weight, operands);
        
        return this;
    }

    public SettingsBuilder WithModulos(double weight, IntegerRange operands)
    {
        _operators[OperatorType.Modulo] = new OperatorSettings(weight, operands);
        
        return this;
    }

    public SettingsBuilder WithPowers(double weight, IntegerRange operands)
    {
        _operators[OperatorType.Power] = new OperatorSettings(weight, operands);
        
        return this;
    }

    public SettingsBuilder WithNegations(double weight, IntegerRange operands)
    {
        _operators[OperatorType.Negation] = new OperatorSettings(weight, operands);
        
        return this;
    }

    public SettingsBuilder WithAbsoluteValues(double weight, IntegerRange operands)
    {
        _operators[OperatorType.AbsoluteValue] = new OperatorSettings(weight, operands);
        
        return this;
    }

    public SettingsBuilder WithFactorials(double weight, IntegerRange operands)
    {
        _operators[OperatorType.Factorial] = new OperatorSettings(weight, operands);
        
        return this;
    }

    public SettingsBuilder WithSquareRoots(double weight, IntegerRange operands)
    {
        _operators[OperatorType.SquareRoot] = new OperatorSettings(weight, operands);
        
        return this;
    }

    public Difficulty Build()
    {
        settings = settings with { Operators = _operators };
        
        return settings;
    }
}

