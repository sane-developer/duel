using Duel.Modules.Engine.Muffs.Glyphs;

namespace Duel.Modules.Engine.Muffs.Expressions;

public static class ExpressionEvaluator
{
    public static int Evaluate(Glyph root)
    {
        return root switch
        {
            Number number => number.Value,
            BinaryOperator binary => EvaluateBinary(binary),
            UnaryOperator unary => EvaluateUnary(unary),
            _ => Situation.Unreachable<int>()
        };
    }

    private static int EvaluateBinary(BinaryOperator op)
    {
        var lhs = Evaluate(op.Lhs);

        var rhs = Evaluate(op.Rhs);

        return op.Type switch
        {
            GlyphType.Add => lhs + rhs,
            GlyphType.Subtract => lhs - rhs,
            GlyphType.Multiply => lhs * rhs,
            GlyphType.Divide => lhs / rhs,
            GlyphType.Modulo => lhs % rhs,
            GlyphType.Power => (int) Math.Pow(lhs, rhs),
            _ => Situation.Unreachable<int>()
        };
    }

    private static int EvaluateUnary(UnaryOperator op)
    {
        var operand = Evaluate(op.Operand);

        return op.Type switch
        {
            GlyphType.Negate => -operand,
            GlyphType.Absolute => Math.Abs(operand),
            GlyphType.SquareRoot => (int) Math.Sqrt(operand),
            GlyphType.Factorial => FactorialFactory.From(operand),
            _ => Situation.Unreachable<int>()
        };
    }
}

file static class FactorialFactory
{
    public static int From(int value)
    {
        return Enumerable.Range(1, value).Aggregate(1, (acc, x) => acc * x);
    }
}