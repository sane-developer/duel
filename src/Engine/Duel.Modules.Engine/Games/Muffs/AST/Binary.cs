namespace Duel.Modules.Engine.Games.Muffs.AST;

public abstract record Binary(Expression Left, Expression Right) : Expression
{
    public enum Type
    {
        Add, 
        Subtract, 
        Multiply, 
        Divide, 
        Modulo, 
        Power
    }

    public static int Precedence(Type type)
    {
        return type switch
        {
            Type.Add or Type.Subtract => 1,
            Type.Multiply or Type.Divide or Type.Modulo => 2,
            Type.Power => 3,
            _ => 0
        };
    }

    public static bool IsRightAssociative(Type type)
    {
        return type is Type.Power;
    }

    public static Binary From(Type type, Expression lhs, Expression rhs) 
    {
        return type switch
        {
            Type.Add => Addition.From(lhs, rhs),
            Type.Subtract => Subtraction.From(lhs, rhs),
            Type.Multiply => Multiplication.From(lhs, rhs),
            Type.Divide => Division.From(lhs, rhs),
            Type.Modulo => Modulo.From(lhs, rhs),
            Type.Power => Power.From(lhs, rhs),
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