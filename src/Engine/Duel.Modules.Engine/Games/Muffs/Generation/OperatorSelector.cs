using Duel.Modules.Engine.Games.Muffs.Representation;

namespace Duel.Modules.Engine.Games.Muffs.Generation;

public sealed class OperatorSelector(GeneratorSettings settings)
{
    public OperatorType SelectBinary(Random rng)
    {
        var weight = rng.NextDouble() * (
            settings.Addition.Weight + 
            settings.Subtraction.Weight + 
            settings.Multiplication.Weight + 
            settings.Division.Weight + 
            settings.Modulo.Weight + 
            settings.Power.Weight
        );

        if ((weight -= settings.Addition.Weight) <= 0)
        {
            return OperatorType.Addition;
        }
        
        if ((weight -= settings.Subtraction.Weight) <= 0)
        {
            return OperatorType.Subtraction;
        }
        
        if ((weight -= settings.Multiplication.Weight) <= 0)
        {
            return OperatorType.Multiplication;
        }
        
        if ((weight -= settings.Division.Weight) <= 0)
        {
            return OperatorType.Division;
        }
        
        if ((weight -= settings.Power.Weight) <= 0)
        {
            return OperatorType.Power;
        }
        
        return OperatorType.Modulo;
    }

    public OperatorType SelectUnary(Random rng)
    {
        var weight = rng.NextDouble() * (
            settings.Negation.Weight +
            settings.AbsoluteValue.Weight + 
            settings.Factorial.Weight + 
            settings.SquareRoot.Weight
        );

        if ((weight -= settings.Negation.Weight) <= 0)
        {
            return OperatorType.Negation;
        }

        if ((weight -= settings.AbsoluteValue.Weight) <= 0)
        {
            return OperatorType.AbsoluteValue;
        }

        if ((weight -= settings.Factorial.Weight) <= 0)
        {
            return OperatorType.Factorial;
        }

        return OperatorType.SquareRoot;
    }
}

