using Duel.Modules.Engine.Games.Muffs.Generation.Difficulties;

namespace Duel.Modules.Engine.Games.Muffs.Generation.Operators;

public sealed class OperatorSelector(Difficulty difficulty)
{
    private readonly double _totalOperatorsWeight = difficulty.Operators
        .Sum(o => o.Value.Weight);

    private readonly double _binaryOperatorsWeight = difficulty.Operators
        .Where(o => IsBinary(o.Key))
        .Sum(o => o.Value.Weight);

    private readonly double _unaryOperatorsWeight = difficulty.Operators
        .Where(o => IsUnary(o.Key))
        .Sum(o => o.Value.Weight);

    private readonly Dictionary<OperatorType, OperatorSettings> _binaryOperators = difficulty.Operators
        .Where(o => IsBinary(o.Key))
        .ToDictionary(o => o.Key, o => o.Value);

    private readonly Dictionary<OperatorType, OperatorSettings> _unaryOperators = difficulty.Operators
        .Where(o => IsUnary(o.Key))
        .ToDictionary(o => o.Key, o => o.Value);

    public OperatorType Select(Random rng)
    {
        var weight = rng.NextDouble() * _totalOperatorsWeight;

        foreach (var (type, settings) in difficulty.Operators)
        {
            if ((weight -= settings.Weight) <= 0)
            {
                return type;
            }
        }

        return Situation.Unreachable<OperatorType>();
    }
    
    public OperatorType SelectBinary(Random rng)
    {
        var weight = rng.NextDouble() * _binaryOperatorsWeight;

        foreach (var (type, settings) in _binaryOperators)
        {
            if ((weight -= settings.Weight) <= 0)
            {
                return type;
            }
        }

        return Situation.Unreachable<OperatorType>();
    }

    public OperatorType SelectUnary(Random rng)
    {
        var weight = rng.NextDouble() * _unaryOperatorsWeight;

        foreach (var (type, settings) in _unaryOperators)
        {
            if ((weight -= settings.Weight) <= 0)
            {
                return type;
            }
        }

        return Situation.Unreachable<OperatorType>();
    }

    private static bool IsBinary(OperatorType type)
    {
        return type is OperatorType.Addition 
            or OperatorType.Subtraction 
            or OperatorType.Multiplication 
            or OperatorType.Division 
            or OperatorType.Modulo 
            or OperatorType.Power;
    }

    private static bool IsUnary(OperatorType type)
    {
        return type is OperatorType.Negation 
            or OperatorType.AbsoluteValue 
            or OperatorType.Factorial 
            or OperatorType.SquareRoot;
    }
}