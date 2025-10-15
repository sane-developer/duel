using Duel.Modules.Engine.Games.Muffs.Expressions.Symbols;

namespace Duel.Modules.Engine.Games.Muffs.Expressions.Vaults;

public sealed class CompositionVault(Range constant)
{
    private List<Expression.Composition> _compositions = [];
    
    private Dictionary<int, List<Expression.Composition>> _compositionsByResults = [];

    public static CompositionVault For(Range constant)
    {
        return new CompositionVault(constant);
    }

    public CompositionVault Register(params Expression.Type[] types)
    {
        foreach (var type in types)
        {
            _compositions.Register(type, constant);
        }

        return this;
    }

    public CompositionVault Filter(Predicate<Expression.Composition> predicate)
    {
        _compositions = [.. _compositions.Where(x => !predicate(x))];

        return this;
    }

    public CompositionVault Compile()
    {
        _compositionsByResults = _compositions.GroupBy(c => c.Result).ToDictionary(g => g.Key, g => g.ToList());

        return this;
    }

    public Expression.Composition GetRandom(int result, Random rng)
    {
        var compositions = _compositionsByResults[result];

        var index = rng.Next(compositions.Count);

        return compositions[index];
    }
}

file static class ExpressionCompositionsRegistrar
{
    public static void Register(this List<Expression.Composition> compositions, Expression.Type type, Range constant)
    {
        switch (type)
        {
            case Expression.Type.Add:
                RegisterAdditions(compositions, constant);
                break;
            case Expression.Type.Subtract:
                RegisterSubtractions(compositions, constant);
                break;
            case Expression.Type.Multiply:
                RegisterMultiplications(compositions, constant);
                break;
            case Expression.Type.Divide:
                RegisterDivisions(compositions, constant);
                break;
            case Expression.Type.Modulo:
                RegisterModulos(compositions, constant);
                break;
            case Expression.Type.Power:
                RegisterPowers(compositions, constant);
                break;
        }
    }

    private static void RegisterAdditions(this List<Expression.Composition> compositions, Range constant)
    {        
        for (var lhs = constant.Start.Value; lhs <= constant.End.Value; lhs++)
        {
            for (var rhs = constant.Start.Value; rhs <= constant.End.Value; rhs++)
            {
                var result = lhs + rhs;

                var composition = Expression.Composition.From(Expression.Type.Add, lhs, rhs, result);
                
                compositions.Add(composition);
            }
        }
    }

    private static void RegisterSubtractions(this List<Expression.Composition> compositions, Range constant)
    {
        for (var lhs = constant.Start.Value; lhs <= constant.End.Value; lhs++)
        {
            for (var rhs = constant.Start.Value; rhs <= constant.End.Value; rhs++)
            {
                var result = lhs - rhs;

                var composition = Expression.Composition.From(Expression.Type.Subtract, lhs, rhs, result);
                
                compositions.Add(composition);
            }
        }
    }

    private static void RegisterMultiplications(this List<Expression.Composition> compositions, Range constant)
    {
        for (var lhs = constant.Start.Value; lhs <= constant.End.Value; lhs++)
        {
            for (var rhs = constant.Start.Value; rhs <= constant.End.Value; rhs++)
            {
                var result = lhs * rhs;

                var composition = Expression.Composition.From(Expression.Type.Multiply, lhs, rhs, result);
                
                compositions.Add(composition);
            }
        }
    }

    private static void RegisterDivisions(this List<Expression.Composition> compositions, Range constant)
    {
        for (var lhs = constant.Start.Value; lhs <= constant.End.Value; lhs++)
        {
            for (var rhs = constant.Start.Value; rhs <= constant.End.Value; rhs++)
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

    private static void RegisterModulos(this List<Expression.Composition> compositions, Range constant)
    {
        for (var lhs = constant.Start.Value; lhs <= constant.End.Value; lhs++)
        {
            for (var rhs = constant.Start.Value; rhs <= constant.End.Value; rhs++)
            {
                var result = lhs % rhs;
                
                var composition = Expression.Composition.From(Expression.Type.Modulo, lhs, rhs, result);
                
                compositions.Add(composition);
            }
        }
    }

    private static void RegisterPowers(this List<Expression.Composition> compositions, Range constant)
    {
        for (var lhs = constant.Start.Value; lhs <= constant.End.Value; lhs++)
        {
            for (var rhs = constant.Start.Value; rhs <= constant.End.Value; rhs++)
            {
                var result = (int) Math.Pow(lhs, rhs);
                
                var composition = Expression.Composition.From(Expression.Type.Power, lhs, rhs, result);
                
                compositions.Add(composition);
            }
        }
    }
}
