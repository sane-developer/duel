using Duel.Modules.Engine.Games.Muffs.Expressions.Symbols;

namespace Duel.Modules.Engine.Games.Muffs.Expressions.Contexts;

public static class ExpressionContextFactory
{
    public static ExpressionContext CreateEasy(Random rng)
    {
        var settings = new ExpressionSettings
        {
            Depth = new Range(1, 10),
            Budget = new Range(1, 10),
            Constant = new Range(1, 100),
            Exponent = new Range(1, 10),
            Operations = [
                Expression.Type.Add, 
                Expression.Type.Subtract, 
                Expression.Type.Multiply
            ]
        };

        return new ExpressionContext(rng, settings);
    }

    public static ExpressionContext CreateMedium(Random rng)
    {
        var settings = new ExpressionSettings
        {
            Depth = new Range(1, 10),
            Constant = new Range(1, 100),
            Exponent = new Range(1, 10),
            Budget = new Range(1, 10),
            Operations = [
                Expression.Type.Add, 
                Expression.Type.Subtract, 
                Expression.Type.Multiply, 
                Expression.Type.Divide, 
                Expression.Type.Power
            ]
        };

        return new ExpressionContext(rng, settings);
    }

    public static ExpressionContext CreateHard(Random rng)
    {
        var settings = new ExpressionSettings
        {
            Depth = new Range(1, 10),
            Constant = new Range(1, 100),
            Exponent = new Range(1, 10),
            Budget = new Range(1, 10),
            Operations = [
                Expression.Type.Add, 
                Expression.Type.Subtract, 
                Expression.Type.Multiply, 
                Expression.Type.Divide, 
                Expression.Type.Modulo, 
                Expression.Type.Power
            ]
        };

        return new ExpressionContext(rng, settings);
    }
}

