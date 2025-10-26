using Duel.Modules.Engine.Games.Muffs.Glyphs;
using Duel.Modules.Engine.Games.Muffs.Glyphs.Literals;
using Duel.Shared.Extensions;

namespace Duel.Modules.Engine.Games.Muffs.Expressions;

public sealed class ExpressionContext(ExpressionSettings settings)
{
    private readonly MuffsCache _cache = new(settings);
    
    public ExpressionSettings Settings => settings;

    public int GetDepth(Random rng)
    {
        return settings.Depth.Random(rng);
    }

    public int GetLength(Random rng)
    {
        return settings.Length.Random(rng);
    }

    public Number GetNumber(int value)
    {
        return _cache.GetNumber(value);
    }

    public int[] GetDivisors(int dividend)
    {
        return _cache.GetDivisors(dividend);
    }

    public Composition[] GetCompositions(int value)
    {
        return _cache.GetCompositions(value);
    }

    public OperatorType GetOperatorType(Random rng)
    {
        var weight = rng.NextDouble() * (
            settings.Addition.Weight + 
            settings.Subtraction.Weight + 
            settings.Multiplication.Weight + 
            settings.Division.Weight + 
            settings.Modulo.Weight + 
            settings.Power.Weight + 
            settings.Negation.Weight +
            settings.AbsoluteValue.Weight + 
            settings.Factorial.Weight + 
            settings.SquareRoot.Weight
        );

        if ((weight -= settings.Addition.Weight) <= 0)
        {
            return OperatorType.Addition;
        }

        if ((weight -= settings.Negation.Weight) <= 0)
        {
            return OperatorType.Negation;
        }

        if ((weight -= settings.Subtraction.Weight) <= 0)
        {
            return OperatorType.Subtraction;
        }

        if ((weight -= settings.AbsoluteValue.Weight) <= 0)
        {
            return OperatorType.AbsoluteValue;
        }

        if ((weight -= settings.Multiplication.Weight) <= 0)
        {
            return OperatorType.Multiplication;
        }

        if ((weight -= settings.Factorial.Weight) <= 0)
        {
            return OperatorType.Factorial;
        }

        if ((weight -= settings.Division.Weight) <= 0)
        {
            return OperatorType.Division;
        }

        if ((weight -= settings.SquareRoot.Weight) <= 0)
        {
            return OperatorType.SquareRoot;
        }

        if ((weight -= settings.Power.Weight) <= 0)
        {
            return OperatorType.Power;
        }

        return OperatorType.Modulo;
    }
}