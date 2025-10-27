namespace Duel.Modules.Engine.Games.Muffs.Representation;

public abstract record Binary(Glyph Lhs, Glyph Rhs) : Operator
{
    public static Binary Create(OperatorType type, Glyph lhs, Glyph rhs)
    {
        return type switch
        {
            OperatorType.Addition => new Add(lhs, rhs),
            OperatorType.Subtraction => new Subtract(lhs, rhs),
            OperatorType.Multiplication => new Multiply(lhs, rhs),
            OperatorType.Division => new Divide(lhs, rhs),
            OperatorType.Modulo => new Modulo(lhs, rhs),
            OperatorType.Power => new Power(lhs, rhs),
            _ => Situation.Unreachable<Binary>()
        };
    }
}

public abstract record Unary(Glyph Operand) : Operator
{
    public static Unary Create(OperatorType type, Glyph operand)
    {
        return type switch
        {
            OperatorType.Negation => new Negate(operand),
            OperatorType.AbsoluteValue => new Absolute(operand),
            OperatorType.Factorial => new Factorial(operand),
            OperatorType.SquareRoot => new SquareRoot(operand),
            _ => Situation.Unreachable<Unary>()
        };
    }
}

public sealed record Add(Glyph Lhs, Glyph Rhs) : Binary(Lhs, Rhs);

public sealed record Subtract(Glyph Lhs, Glyph Rhs) : Binary(Lhs, Rhs);

public sealed record Multiply(Glyph Lhs, Glyph Rhs) : Binary(Lhs, Rhs);

public sealed record Divide(Glyph Lhs, Glyph Rhs) : Binary(Lhs, Rhs);

public sealed record Modulo(Glyph Lhs, Glyph Rhs) : Binary(Lhs, Rhs);

public sealed record Power(Glyph Lhs, Glyph Rhs) : Binary(Lhs, Rhs);

public sealed record Negate(Glyph Operand) : Unary(Operand);

public sealed record Absolute(Glyph Operand) : Unary(Operand);

public sealed record SquareRoot(Glyph Operand) : Unary(Operand);

public sealed record Factorial(Glyph Operand) : Unary(Operand);