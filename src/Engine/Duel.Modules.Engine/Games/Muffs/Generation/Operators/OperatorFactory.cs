using Duel.Modules.Engine.Games.Muffs.Representation;

namespace Duel.Modules.Engine.Games.Muffs.Generation.Operators;

public static class OperatorFactory
{
    public static Operator Binary(OperatorType type, Glyph lhs, Glyph rhs)
    {
        return type switch
        {
            OperatorType.Addition => new Add(lhs, rhs),
            OperatorType.Subtraction => new Subtract(lhs, rhs),
            OperatorType.Multiplication => new Multiply(lhs, rhs),
            OperatorType.Division => new Divide(lhs, rhs),
            OperatorType.Modulo => new Modulo(lhs, rhs),
            OperatorType.Power => new Power(lhs, rhs),
            _ => Situation.Unreachable<Operator>()
        };
    }

    public static Operator Unary(OperatorType type, Glyph operand)
    {
        return type switch
        {
            OperatorType.Negation => new Negate(operand),
            OperatorType.Absolute => new Absolute(operand),
            OperatorType.Factorial => new Factorial(operand),
            OperatorType.SquareRoot => new SquareRoot(operand),
            _ => Situation.Unreachable<Operator>()
        };
    }
}