using Duel.Modules.Engine.Games.Muffs.AST;
using Duel.Modules.Engine.Games.Muffs.AST.Literals;
using Duel.Modules.Engine.Games.Muffs.AST.Operators;
using Duel.Shared.Extensions;

namespace Duel.Modules.Engine.Games.Muffs;

public sealed class ExpressionEvaluator
{
    public static int Evaluate(Symbol root)
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
        var left = Evaluate(binary.Lhs);

        var right = Evaluate(binary.Rhs);

        return (int) Math.Pow(left, right);
    }

    private static int Negate(Unary unary)
    {
        return -Evaluate(unary.Operand);
    }

    private static int Absolute(Unary unary)
    {
        var value = Evaluate(unary.Operand);

        return Math.Abs(value);
    }

    private static int Factorial(Unary unary)
    {
        return Evaluate(unary.Operand).Factorial();
    }

    private static int SquareRoot(Unary unary)
    {
        var value = Evaluate(unary.Operand);

        return (int) Math.Sqrt(value);
    }
}