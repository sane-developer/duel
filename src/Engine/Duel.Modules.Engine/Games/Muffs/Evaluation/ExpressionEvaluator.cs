using Duel.Modules.Engine.Games.Muffs.Representation;

namespace Duel.Modules.Engine.Games.Muffs.Evaluation;

public static class ExpressionEvaluator
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
        var lhs = Evaluate(binary.Lhs);

        var rhs = Evaluate(binary.Rhs);

        return (int) Math.Pow(lhs, rhs);
    }

    private static int Negate(Unary unary)
    {
        return -Evaluate(unary.Operand);
    }

    private static int Absolute(Unary unary)
    {
        var operand = Evaluate(unary.Operand);

        return Math.Abs(operand);
    }

    private static int Factorial(Unary unary)
    {
        var operand = Evaluate(unary.Operand);

        return operand.Factorial();
    }

    private static int SquareRoot(Unary unary)
    {
        var operand = Evaluate(unary.Operand);

        return (int) Math.Sqrt(operand);
    }
}

