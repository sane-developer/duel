using Duel.Modules.Engine.Games.Muffs.Expressions.AST;

namespace Duel.Modules.Engine.Games.Muffs.Expressions.Compositions;

public static class CompositionFactory
{
    public static IEnumerable<Composition> FromAdditions(Range constant)
    {
        for (var lhs = constant.Start.Value; lhs <= constant.End.Value; lhs++)
        {
            for (var rhs = constant.Start.Value; rhs <= constant.End.Value; rhs++)
            {
                yield return Composition.From(Expression.Operator.Add, lhs, rhs, lhs + rhs);
            }
        }
    }

    public static IEnumerable<Composition> FromSubtractions(Range constant)
    {
        for (var lhs = constant.Start.Value; lhs <= constant.End.Value; lhs++)
        {
            for (var rhs = constant.Start.Value; rhs <= constant.End.Value; rhs++)
            {
                yield return Composition.From(Expression.Operator.Subtract, lhs, rhs, lhs - rhs);
            }
        }
    }

    public static IEnumerable<Composition> FromMultiplications(Range constant)
    {
        for (var lhs = constant.Start.Value; lhs <= constant.End.Value; lhs++)
        {
            for (var rhs = constant.Start.Value; rhs <= constant.End.Value; rhs++)
            {
                yield return Composition.From(Expression.Operator.Multiply, lhs, rhs, lhs * rhs);
            }
        }
    }

    public static IEnumerable<Composition> FromDivisions(Range constant)
    {
        for (var lhs = constant.Start.Value; lhs <= constant.End.Value; lhs++)
        {
            for (var rhs = constant.Start.Value; rhs <= constant.End.Value; rhs++)
            {
                if (rhs is not 0)
                {
                    yield return Composition.From(Expression.Operator.Divide, lhs, rhs, lhs / rhs);
                }
            }
        }
    }

    public static IEnumerable<Composition> FromPowers(Range constant)
    {
        for (var lhs = constant.Start.Value; lhs <= constant.End.Value; lhs++)
        {
            for (var rhs = constant.Start.Value; rhs <= constant.End.Value; rhs++)
            {
                yield return Composition.From(Expression.Operator.Power, lhs, rhs, (int) Math.Pow(lhs, rhs));
            }
        }
    }
    
    public static IEnumerable<Composition> FromModulos(Range constant)
    {
        for (var lhs = constant.Start.Value; lhs <= constant.End.Value; lhs++)
        {
            for (var rhs = constant.Start.Value; rhs <= constant.End.Value; rhs++)
            {
                yield return Composition.From(Expression.Operator.Modulo, lhs, rhs, lhs % rhs);
            }
        }
    }
}