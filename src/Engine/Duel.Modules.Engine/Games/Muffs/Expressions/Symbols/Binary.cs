namespace Duel.Modules.Engine.Games.Muffs.Expressions.Symbols;

public abstract record Binary(Expression Left, Expression Right) : Expression
{
    public static Binary From(Operator type, Expression lhs, Expression rhs) 
    {
        return type switch
        {
            Operator.Add => Addition.From(lhs, rhs),
            Operator.Subtract => Subtraction.From(lhs, rhs),
            Operator.Multiply => Multiplication.From(lhs, rhs),
            Operator.Divide => Division.From(lhs, rhs),
            Operator.Modulo => Modulo.From(lhs, rhs),
            Operator.Power => Power.From(lhs, rhs),
            _ => Situation.Unreachable<Binary>()
        };
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

public sealed record Modulo(Expression Left, Expression Right) : Binary(Left, Right)
{
    public static Modulo From(Expression lhs, Expression rhs)
    {
        return new Modulo(lhs, rhs);
    }
}