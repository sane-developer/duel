namespace Duel.Modules.Engine.Games.Muffs.Expressions.AST;

public abstract record Unary(Expression Operand) : Expression
{
    public static Unary From(Operator type, Expression operand)
    {
        return type switch
        {
            Operator.Negate => Negation.From(operand),
            Operator.Absolute => Absolute.From(operand),
            Operator.SquareRoot => SquareRoot.From(operand),
            Operator.Factorial => Factorial.From(operand),
            _ => Situation.Unreachable<Unary>()
        };
    }
}

public sealed record Negation(Expression Operand) : Unary(Operand)
{
    public static Negation From(Expression operand)
    {
        return new Negation(operand);
    }
}

public sealed record Absolute(Expression Operand) : Unary(Operand)
{
    public static Absolute From(Expression operand)
    {
        return new Absolute(operand);
    }
}

public sealed record SquareRoot(Expression Operand) : Unary(Operand)
{
    public static SquareRoot From(Expression operand)
    {
        return new SquareRoot(operand);
    }
}

public sealed record Factorial(Expression Operand) : Unary(Operand)
{
    public static Factorial From(Expression operand)
    {
        return new Factorial(operand);
    }
}