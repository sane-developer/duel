namespace Duel.Modules.Engine.Games.Muffs.AST;

public abstract record Binary(Expression Left, Expression Right) : Expression
{
    public static int Precedence(ExpressionType type)
    {
        return type switch
        {
            ExpressionType.Add or ExpressionType.Subtract => 1,
            ExpressionType.Multiply or ExpressionType.Divide or ExpressionType.Modulo => 2,
            ExpressionType.Power => 3,
            _ => 0
        };
    }

    public static bool IsRightAssociative(ExpressionType type)
    {
        return type is ExpressionType.Power;
    }

    public static Binary From(ExpressionType type, Expression lhs, Expression rhs) 
    {
        return type switch
        {
            ExpressionType.Add => Addition.From(lhs, rhs),
            ExpressionType.Subtract => Subtraction.From(lhs, rhs),
            ExpressionType.Multiply => Multiplication.From(lhs, rhs),
            ExpressionType.Divide => Division.From(lhs, rhs),
            ExpressionType.Modulo => Modulo.From(lhs, rhs),
            ExpressionType.Power => Power.From(lhs, rhs),
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