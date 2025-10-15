using Duel.Modules.Engine.Games.Muffs.Expressions.Symbols;

namespace Duel.Modules.Engine.Games.Muffs.Expressions.Vaults;

public sealed class CompositionVault(Range constant)
{
    private List<Expression.Composition> _compositions = [];
    
    private Dictionary<int, Expression.Composition[]> _compositionsByResults = [];

    public static CompositionVault For(Range constant)
    {
        return new CompositionVault(constant);
    }

    public CompositionVault Register(params Expression.Operator[] types)
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
        _compositionsByResults = _compositions.GroupBy(c => c.Result).ToDictionary(g => g.Key, g => g.ToArray());

        return this;
    }

    public Expression.Composition GetRandom(int result, Random rng)
    {
        var compositions = _compositionsByResults[result];

        var index = rng.Next(compositions.Length);

        return compositions[index];
    }
}

file static class ExpressionCompositionsRegistrar
{
    public static void Register(this List<Expression.Composition> compositions, Expression.Operator type, Range constant)
    {
        switch (type)
        {
            case Expression.Operator.Add:
                compositions.RegisterAdditions(constant);
                break;
            case Expression.Operator.Subtract:
                compositions.RegisterSubtractions(constant);
                break;
            case Expression.Operator.Multiply:
                compositions.RegisterMultiplications(constant);
                break;
            case Expression.Operator.Divide:
                compositions.RegisterDivisions(constant);
                break;
            case Expression.Operator.Modulo:
                compositions.RegisterModulos(constant);
                break;
            case Expression.Operator.Power:
                compositions.RegisterPowers(constant);
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

                var composition = Expression.Composition.From(Expression.Operator.Add, lhs, rhs, result);
                
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

                var composition = Expression.Composition.From(Expression.Operator.Subtract, lhs, rhs, result);
                
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

                var composition = Expression.Composition.From(Expression.Operator.Multiply, lhs, rhs, result);
                
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

                    var composition = Expression.Composition.From(Expression.Operator.Divide, lhs, rhs, result);
                    
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
                
                var composition = Expression.Composition.From(Expression.Operator.Modulo, lhs, rhs, result);
                
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
                
                var composition = Expression.Composition.From(Expression.Operator.Power, lhs, rhs, result);
                
                compositions.Add(composition);
            }
        }
    }
}
