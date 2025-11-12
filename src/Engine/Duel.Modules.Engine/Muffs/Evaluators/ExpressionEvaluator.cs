using Duel.Modules.Engine.Muffs.Glyphs;

namespace Duel.Modules.Engine.Muffs.Evaluators;

public static class ExpressionEvaluator
{
    public static int Evaluate(Glyph root)
    {
        return root.Type switch
        {
            GlyphType.Number => Number(root.AsNumber()),
            GlyphType.Add => Add(root.AsBinary()),
            GlyphType.Subtract => Subtract(root.AsBinary()),
            GlyphType.Multiply => Multiply(root.AsBinary()),
            GlyphType.Divide => Divide(root.AsBinary()),
            GlyphType.Modulo => Modulo(root.AsBinary()),
            GlyphType.Power => Power(root.AsBinary()),
            GlyphType.Negate => Negate(root.AsUnary()),
            GlyphType.Absolute => Absolute(root.AsUnary()),
            GlyphType.SquareRoot => SquareRoot(root.AsUnary()),
            GlyphType.Factorial => Factorial(root.AsUnary()),
            _ => Situation.Unreachable<int>()
        };
    }

    private static int Number(Number node)
    {
        return node.Value;
    }

    private static int Add(BinaryOperator node)
    {
        var lhs = Evaluate(node.Lhs);

        var rhs = Evaluate(node.Rhs);

        return lhs + rhs;
    }

    private static int Subtract(BinaryOperator node)
    {
        var lhs = Evaluate(node.Lhs);

        var rhs = Evaluate(node.Rhs);

        return lhs - rhs;
    }

    private static int Multiply(BinaryOperator node)
    {
        var lhs = Evaluate(node.Lhs);

        var rhs = Evaluate(node.Rhs);

        return lhs * rhs;
    }

    private static int Divide(BinaryOperator node)
    {
        var lhs = Evaluate(node.Lhs);

        var rhs = Evaluate(node.Rhs);

        return lhs / rhs;
    }

    private static int Modulo(BinaryOperator node)
    {
        var lhs = Evaluate(node.Lhs);

        var rhs = Evaluate(node.Rhs);

        return lhs % rhs;
    }

    private static int Power(BinaryOperator node)
    {
        var lhs = Evaluate(node.Lhs);

        var rhs = Evaluate(node.Rhs);

        return (int) Math.Pow(lhs, rhs);
    }

    private static int Negate(UnaryOperator node)
    {
        return -Evaluate(node.Operand);
    }

    private static int Absolute(UnaryOperator node)
    {
        var operand = Evaluate(node.Operand);

        return Math.Abs(operand);
    }

    private static int SquareRoot(UnaryOperator node)
    {
        var operand = Evaluate(node.Operand);

        return (int) Math.Sqrt(operand);
    }

    private static int Factorial(UnaryOperator node)
    {
        var operand = Evaluate(node.Operand);

        return FactorialFactory.From(operand);
    }
}

file static class FactorialFactory
{
    public static int From(int value)
    {
        return Enumerable.Range(1, value).Aggregate(1, (acc, x) => acc * x);
    }
}