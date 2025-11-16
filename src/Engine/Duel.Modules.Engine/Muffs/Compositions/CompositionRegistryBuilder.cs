using Duel.Modules.Engine.Muffs.Glyphs;

namespace Duel.Modules.Engine.Muffs.Compositions;

public sealed class CompositionRegistryBuilder
{
    private readonly List<Composition> _compositions = [];

    public CompositionRegistryBuilder WithAdditions(int minimum, int maximum)
    {
        var compositions = AdditionCompositionRegistry.From(minimum, maximum);

        Populate(compositions);

        return this;
    }

    public CompositionRegistryBuilder WithSubtractions(int minimum, int maximum)
    {
        var compositions = SubtractionCompositionRegistry.From(minimum, maximum);

        Populate(compositions);

        return this;
    }

    public CompositionRegistryBuilder WithMultiplications(int minimum, int maximum)
    {
        var compositions = MultiplicationCompositionRegistry.From(minimum, maximum);

        Populate(compositions);

        return this;
    }

    public CompositionRegistryBuilder WithDivisions(int minimum, int maximum)
    {
        var compositions = DivisionCompositionRegistry.From(minimum, maximum);

        Populate(compositions);

        return this;
    }

    public CompositionRegistryBuilder WithModulos(int minimum, int maximum)
    {
        var compositions = ModuloCompositionRegistry.From(minimum, maximum);

        Populate(compositions);

        return this;
    }

    public CompositionRegistryBuilder WithPowers(int baseMinimum, int baseMaximum, int exponentMinimum, int exponentMaximum)
    {
        var compositions = PowerCompositionRegistry.From(baseMinimum, baseMaximum, exponentMinimum, exponentMaximum);

        Populate(compositions);

        return this;
    }

    public CompositionRegistryBuilder WithNegations(int minimum, int maximum)
    {
        var compositions = NegationCompositionRegistry.From(minimum, maximum);

        Populate(compositions);

        return this;
    }

    public CompositionRegistryBuilder WithAbsoluteValues(int minimum, int maximum)
    {
        var compositions = AbsoluteValueCompositionRegistry.From(minimum, maximum);

        Populate(compositions);

        return this;
    }

    public CompositionRegistryBuilder WithSquareRoots(int minimum, int maximum)
    {
        var compositions = SquareRootCompositionRegistry.From(minimum, maximum);

        Populate(compositions);

        return this;
    }

    public CompositionRegistryBuilder WithFactorials(int minimum, int maximum)
    {
        var compositions = FactorialCompositionRegistry.From(minimum, maximum);

        Populate(compositions);

        return this;
    }

    public CompositionRegistry Build()
    {
        return new CompositionRegistry(_compositions);
    }

    private void Populate(IEnumerable<Composition> compositions)
    {
        _compositions.AddRange(compositions);
    }
}

file static class AdditionCompositionRegistry
{
    public static IEnumerable<Composition> From(int minimum, int maximum)
    {
        for (var lhs = minimum; lhs <= maximum; lhs++)
        {
            for (var rhs = minimum; rhs <= maximum; rhs++)
            {
                yield return BinaryComposition.From(GlyphType.Add, lhs, rhs, lhs + rhs);
            }
        }
    }
}

file static class SubtractionCompositionRegistry
{
    public static IEnumerable<Composition> From(int minimum, int maximum)
    {
        for (var lhs = minimum; lhs <= maximum; lhs++)
        {
            for (var rhs = minimum; rhs <= maximum; rhs++)
            {
                yield return BinaryComposition.From(GlyphType.Subtract, lhs, rhs, lhs - rhs);
            }
        }
    }
}

file static class MultiplicationCompositionRegistry
{
    public static IEnumerable<Composition> From(int minimum, int maximum)
    {
        for (var lhs = minimum; lhs <= maximum; lhs++)
        {
            for (var rhs = minimum; rhs <= maximum; rhs++)
            {
                yield return BinaryComposition.From(GlyphType.Multiply, lhs, rhs, lhs * rhs);
            }
        }
    }
}

file static class DivisionCompositionRegistry
{
    public static IEnumerable<Composition> From(int minimum, int maximum)
    {
        for (var lhs = minimum; lhs <= maximum; lhs++)
        {
            for (var rhs = minimum; rhs <= maximum; rhs++)
            {
                if (rhs is not 0)
                {
                    yield return BinaryComposition.From(GlyphType.Divide, lhs, rhs, lhs / rhs);
                }
            }
        }
    }
}

file static class ModuloCompositionRegistry
{
    public static IEnumerable<Composition> From(int minimum, int maximum)
    {
        for (var lhs = minimum; lhs <= maximum; lhs++)
        {
            for (var rhs = minimum; rhs <= maximum; rhs++)
            {
                if (rhs is >= 1)
                {
                    yield return BinaryComposition.From(GlyphType.Modulo, lhs, rhs, lhs % rhs);
                }
            }
        }
    }
}

file static class PowerCompositionRegistry
{
    public static IEnumerable<Composition> From(int baseMinimum, int baseMaximum, int exponentMinimum, int exponentMaximum)
    {
        for (var lhs = baseMinimum; lhs <= baseMaximum; lhs++)
        {
            for (var rhs = exponentMinimum; rhs <= exponentMaximum; rhs++)
            {
                var result = Math.Pow(lhs, rhs);

                if (result is >= int.MinValue and <= int.MaxValue)
                {
                    yield return BinaryComposition.From(GlyphType.Power, lhs, rhs, (int) result);
                }
            }
        }
    }
}

file static class NegationCompositionRegistry
{
    public static IEnumerable<Composition> From(int minimum, int maximum)
    {
        for (var operand = minimum; operand <= maximum; operand++)
        {
            yield return UnaryComposition.From(GlyphType.Negate, operand, -operand);
        }
    }
}

file static class AbsoluteValueCompositionRegistry
{
    public static IEnumerable<Composition> From(int minimum, int maximum)
    {
        for (var operand = minimum; operand <= maximum; operand++)
        {
            yield return UnaryComposition.From(GlyphType.Absolute, operand, Math.Abs(operand));
        }
    }
}

file static class SquareRootCompositionRegistry
{
    public static IEnumerable<Composition> From(int minimum, int maximum)
    {
        for (var operand = minimum; operand <= maximum; operand++)
        {
            yield return UnaryComposition.From(GlyphType.SquareRoot, operand, (int) Math.Sqrt(operand));
        }
    }
}

file static class FactorialCompositionRegistry
{
    public static IEnumerable<Composition> From(int minimum = 0, int maximum = 7)
    {
        for (var operand = minimum; operand <= maximum; operand++)
        {
            yield return UnaryComposition.From(GlyphType.Factorial, operand, FactorialFactory.From(operand));
        }
    }
}

file static class FactorialFactory
{
    public static int From(int value)
    {
        return Enumerable.Range(1, value).Aggregate(1, (acc, x) => acc * x);
    }
}