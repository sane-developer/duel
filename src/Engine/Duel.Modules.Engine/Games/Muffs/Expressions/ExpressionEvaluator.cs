using Duel.Modules.Engine.Games.Muffs.Glyphs;
using Duel.Modules.Engine.Games.Muffs.Glyphs.Literals;
using Duel.Modules.Engine.Games.Muffs.Glyphs.Operators;
using Duel.Shared.Extensions;

namespace Duel.Modules.Engine.Games.Muffs.Expressions;

public sealed class ExpressionEvaluator
{
    public static int Evaluate(Glyph root)
    {
        return root switch
        {
            Number node => node.Value,
            Add node => Add(node),
            Subtract node => Subtract(node),
            Multiply node => Multiply(node),
            Divide node => Divide(node),
            Modulo node => Modulo(node),
            Power node => Power(node),
            Negate node => Negate(node),
            Absolute node => Absolute(node),
            Factorial node => Factorial(node),
            SquareRoot node => SquareRoot(node),
            _ => Situation.Unreachable<int>()
        };
    }

    private static int Add(Binary binary)
    {
        return Evaluate(binary.Lhs) + Evaluate(binary.Rhs);
    }

    private static int Subtract(Binary binary)
    {
        return Evaluate(binary.Lhs) - Evaluate(binary.Rhs);
    }

    private static int Multiply(Binary binary)
    {
        return Evaluate(binary.Lhs) * Evaluate(binary.Rhs);
    }

    private static int Divide(Binary binary)
    {
        return Evaluate(binary.Lhs) / Evaluate(binary.Rhs);
    }

    private static int Modulo(Binary binary)
    {
        return Evaluate(binary.Lhs) % Evaluate(binary.Rhs);
    }

    private static int Power(Binary binary)
    {
        return Evaluate(binary.Lhs).Power(Evaluate(binary.Rhs));
    }

    private static int Negate(Unary unary)
    {
        return Evaluate(unary.Operand).Negate();
    }

    private static int Absolute(Unary unary)
    {
        return Evaluate(unary.Operand).Absolute();
    }

    private static int Factorial(Unary unary)
    {
        return Evaluate(unary.Operand).Factorial();
    }

    private static int SquareRoot(Unary unary)
    {
        return Evaluate(unary.Operand).SquareRoot();
    }
}