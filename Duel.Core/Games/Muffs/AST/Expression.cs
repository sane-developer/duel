namespace Duel.Core.Games.Muffs.AST;

public abstract record Expression;

public abstract record Binary(Expression Left, Expression Right) : Expression;

public sealed record Constant(int Value) : Expression
{
    public static Constant From(int value)
    {
        return new Constant(value);
    }
}

public sealed record Addition(Expression Left, Expression Right) : Binary(Left, Right)
{
    public static Addition From(Expression left, Expression right)
    {
        return new Addition(left, right);
    }
}

public sealed record Subtraction(Expression Left, Expression Right) : Binary(Left, Right)
{
    public static Subtraction From(Expression left, Expression right)
    {
        return new Subtraction(left, right);
    }   
}

public sealed record Multiplication(Expression Left, Expression Right) : Binary(Left, Right)
{
    public static Multiplication From(Expression left, Expression right)
    {
        return new Multiplication(left, right);
    }   
}

public sealed record Division(Expression Left, Expression Right) : Binary(Left, Right)
{
    public static Division From(Expression left, Expression right)
    {
        return new Division(left, right);
    }   
}

public sealed record Power(Expression Left, Expression Right) : Binary(Left, Right)
{
    public static Power From(Expression left, Expression right)
    {
        return new Power(left, right);
    }
}