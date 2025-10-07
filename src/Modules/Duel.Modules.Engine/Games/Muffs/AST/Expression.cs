using Duel.Shared.Common;

namespace Duel.Core.Games.Muffs.AST;

public abstract record Expression
{
    public T As<T>() where T : Expression
    {
        return (T) this;
    }
}

public abstract record Binary(Expression Left, Expression Right) : Expression
{
    public static Binary From(Operator.Code code, Expression lhs, Expression rhs) 
    {
        return code switch
        {
            Operator.Code.Add => Addition.From(lhs, rhs),
            Operator.Code.Subtract => Subtraction.From(lhs, rhs),
            Operator.Code.Multiply => Multiplication.From(lhs, rhs),
            Operator.Code.Divide => Division.From(lhs, rhs),
            Operator.Code.Power => Power.From(lhs, rhs),
            _ => Situation.Unreachable<Binary>()
        };
    }
}

public sealed record Constant(int Value) : Expression
{
    public static Constant From(int value)
    {
        return new Constant(value);
    }
}

public sealed record Addition(Expression Left, Expression Right) : Binary(Left, Right)
{
    public static Addition From(Expression lhs, Expression rhs)
    {
        return new Addition(lhs, rhs);
    }
}

public sealed record Subtraction(Expression Left, Expression Right) : Binary(Left, Right)
{
    public static Subtraction From(Expression lhs, Expression rhs)
    {
        return new Subtraction(lhs, rhs);
    }   
}

public sealed record Multiplication(Expression Left, Expression Right) : Binary(Left, Right)
{
    public static Multiplication From(Expression lhs, Expression rhs)
    {
        return new Multiplication(lhs, rhs);
    }   
}

public sealed record Division(Expression Left, Expression Right) : Binary(Left, Right)
{
    public static Division From(Expression lhs, Expression rhs)
    {
        return new Division(lhs, rhs);
    }   
}

public sealed record Power(Expression Left, Expression Right) : Binary(Left, Right)
{
    public static Power From(Expression lhs, Expression rhs)
    {
        return new Power(lhs, rhs);
    }
}