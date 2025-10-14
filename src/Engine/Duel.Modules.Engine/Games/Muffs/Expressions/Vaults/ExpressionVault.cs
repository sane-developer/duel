using Duel.Modules.Engine.Games.Muffs.Expressions.Symbols;

namespace Duel.Modules.Engine.Games.Muffs.Expressions.Vaults;

public sealed class ExpressionVault(int minimum, int maximum)
{
    private List<Expression.Composition> _compositions = [];
    
    private Dictionary<int, List<Expression.Composition>> _compositionsByResults = [];

    public static ExpressionVault For(int minimum, int maximum)
    {
        return new ExpressionVault(minimum, maximum);
    }

    public ExpressionVault Register(params Expression.Type[] types)
    {
        foreach (var type in types)
        {
            switch (type)
            {
                case Expression.Type.Add:
                    ExpressionCompositionsProvider.RegisterAdditions(_compositions, minimum, maximum);
                    break;
                case Expression.Type.Subtract:
                    ExpressionCompositionsProvider.RegisterSubtractions(_compositions, minimum, maximum);
                    break;
                case Expression.Type.Multiply:
                    ExpressionCompositionsProvider.RegisterMultiplications(_compositions, minimum, maximum);
                    break;
                case Expression.Type.Divide:
                    ExpressionCompositionsProvider.RegisterDivisions(_compositions, minimum, maximum);
                    break;
                case Expression.Type.Modulo:
                    ExpressionCompositionsProvider.RegisterModulos(_compositions, minimum, maximum);
                    break;
                case Expression.Type.Power:
                    ExpressionCompositionsProvider.RegisterPowers(_compositions, minimum, maximum);
                    break;
            }
        }

        return this;
    }

    public ExpressionVault Filter(Predicate<Expression.Composition> predicate)
    {
        _compositions = [.. _compositions.Where(x => !predicate(x))];

        return this;
    }

    public ExpressionVault Compile()
    {
        _compositionsByResults = _compositions.GroupBy(c => c.Result).ToDictionary(g => g.Key, g => g.ToList());

        return this;
    }

    public Expression.Composition GetRandomComposition(Random rng, int result)
    {
        var compositions = _compositionsByResults[result];

        var index = rng.Next(compositions.Count);

        return compositions[index];
    }
}

file static class ExpressionCompositionsProvider
{
    public static void RegisterAdditions(List<Expression.Composition> compositions, int minimum, int maximum)
    {        
        for (var lhs = minimum; lhs <= maximum; lhs++)
        {
            for (var rhs = minimum; rhs <= maximum; rhs++)
            {
                var result = lhs + rhs;

                var composition = Expression.Composition.From(Expression.Type.Add, lhs, rhs, result);
                
                compositions.Add(composition);
            }
        }
    }

    public static void RegisterSubtractions(List<Expression.Composition> compositions, int minimum, int maximum)
    {
        for (var lhs = minimum; lhs <= maximum; lhs++)
        {
            for (var rhs = minimum; rhs <= maximum; rhs++)
            {
                var result = lhs - rhs;

                var composition = Expression.Composition.From(Expression.Type.Subtract, lhs, rhs, result);
                
                compositions.Add(composition);
            }
        }
    }

    public static void RegisterMultiplications(List<Expression.Composition> compositions, int minimum, int maximum)
    {
        for (var lhs = minimum; lhs <= maximum; lhs++)
        {
            for (var rhs = minimum; rhs <= maximum; rhs++)
            {
                var result = lhs * rhs;

                var composition = Expression.Composition.From(Expression.Type.Multiply, lhs, rhs, result);
                
                compositions.Add(composition);
            }
        }
    }

    public static void RegisterDivisions(List<Expression.Composition> compositions, int minimum, int maximum)
    {
        for (var lhs = minimum; lhs <= maximum; lhs++)
        {
            for (var rhs = minimum; rhs <= maximum; rhs++)
            {
                if (rhs is not 0)
                {
                    var result = lhs / rhs;

                    var composition = Expression.Composition.From(Expression.Type.Divide, lhs, rhs, result);
                    
                    compositions.Add(composition);
                }
            }
        }
    }

    public static void RegisterModulos(List<Expression.Composition> compositions, int minimum, int maximum)
    {
        for (var lhs = minimum; lhs <= maximum; lhs++)
        {
            for (var rhs = minimum; rhs <= maximum; rhs++)
            {
                var result = lhs % rhs;
                
                var composition = Expression.Composition.From(Expression.Type.Modulo, lhs, rhs, result);
                
                compositions.Add(composition);
            }
        }
    }

    public static void RegisterPowers(List<Expression.Composition> compositions, int minimum, int maximum)
    {
        for (var lhs = minimum; lhs <= maximum; lhs++)
        {
            for (var rhs = minimum; rhs <= maximum; rhs++)
            {
                var result = (int) Math.Pow(lhs, rhs);
                
                var composition = Expression.Composition.From(Expression.Type.Power, lhs, rhs, result);
                
                compositions.Add(composition);
            }
        }
    }
}
