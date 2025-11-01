using Duel.Modules.Engine.Games.Muffs.Generation.Operators;
using Duel.Shared.Ranges;

namespace Duel.Modules.Engine.Games.Muffs.Generation.Difficulties;

public sealed class DifficultyBuilder(Difficulty difficulty)
{
    private readonly Dictionary<OperatorType, OperatorSettings> _operators = [];

    public static DifficultyBuilder New()
    {
        var settings = new Difficulty();

        return new DifficultyBuilder(settings);
    }

    public DifficultyBuilder WithDepth(IntegerRange depth)
    {
        difficulty = difficulty with { Depth = depth };
        
        return this;
    }

    public DifficultyBuilder WithLength(IntegerRange length)
    {
        difficulty = difficulty with { Length = length };
        
        return this;
    }

    public DifficultyBuilder WithAdditions(double weight, IntegerRange operands)
    {
        _operators[OperatorType.Addition] = new OperatorSettings(weight, operands);
        
        return this;
    }

    public DifficultyBuilder WithSubtractions(double weight, IntegerRange operands)
    {
        _operators[OperatorType.Subtraction] = new OperatorSettings(weight, operands);
        
        return this;
    }

    public DifficultyBuilder WithMultiplications(double weight, IntegerRange operands)
    {
        _operators[OperatorType.Multiplication] = new OperatorSettings(weight, operands);
        
        return this;
    }

    public DifficultyBuilder WithDivisions(double weight, IntegerRange operands)
    {
        _operators[OperatorType.Division] = new OperatorSettings(weight, operands);
        
        return this;
    }

    public DifficultyBuilder WithModulos(double weight, IntegerRange operands)
    {
        _operators[OperatorType.Modulo] = new OperatorSettings(weight, operands);
        
        return this;
    }

    public DifficultyBuilder WithPowers(double weight, IntegerRange operands)
    {
        _operators[OperatorType.Power] = new OperatorSettings(weight, operands);
        
        return this;
    }

    public DifficultyBuilder WithNegations(double weight, IntegerRange operands)
    {
        _operators[OperatorType.Negation] = new OperatorSettings(weight, operands);
        
        return this;
    }

    public DifficultyBuilder WithAbsolute(double weight, IntegerRange operands)
    {
        _operators[OperatorType.Absolute] = new OperatorSettings(weight, operands);
        
        return this;
    }

    public DifficultyBuilder WithFactorials(double weight, IntegerRange operands)
    {
        _operators[OperatorType.Factorial] = new OperatorSettings(weight, operands);
        
        return this;
    }

    public DifficultyBuilder WithSquareRoots(double weight, IntegerRange operands)
    {
        _operators[OperatorType.SquareRoot] = new OperatorSettings(weight, operands);
        
        return this;
    }

    public Difficulty Build()
    {
        difficulty = difficulty with { Operators = _operators };
        
        return difficulty;
    }
}