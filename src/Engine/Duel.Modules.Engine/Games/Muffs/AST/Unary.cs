namespace Duel.Modules.Engine.Games.Muffs.AST;

public abstract record Unary(Expression Operand) : Expression
{
    public enum Type
    {
        Negate, 
        Abs, 
        Sqrt, 
        Factorial
    }

    public static Unary From(Type type, Expression operand)
    {
        return type switch
        {
            Type.Negate => Negation.From(operand),
            Type.Abs => Abs.From(operand),
            Type.Sqrt => SquareRoot.From(operand),
            Type.Factorial => Factorial.From(operand),
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

public sealed record Abs(Expression Operand) : Unary(Operand)
{
    public static Abs From(Expression operand)
    {
        return new Abs(operand);
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