namespace Duel.Core.Games.Muffs;

public enum OperatorType
{
    Null, Add, Subtract, Multiply, Divide, Power
}

public readonly record struct Operator(OperatorType Type, Func<int, int, int> Callback)
{
    public static readonly Operator Null = new(
        OperatorType.Null, (_, _) => int.MaxValue
    );
    
    public static readonly Operator Add = new(
        OperatorType.Add, (lhs, rhs) => lhs + rhs
    );
    
    public static readonly Operator Subtract = new(
        OperatorType.Subtract, (lhs, rhs) => lhs - rhs
    );
    
    public static readonly Operator Multiply = new(
        OperatorType.Multiply, (lhs, rhs) => lhs * rhs
    );
    
    public static readonly Operator Divide = new(
        OperatorType.Divide, (lhs, rhs) => lhs / rhs
    );
    
    public static readonly Operator Power = new(
        OperatorType.Power, (lhs, rhs) => Convert.ToInt32(Math.Pow(lhs, rhs))
    );
}

public readonly record struct Symbol(int Value, Operator Operator, bool IsOperator)
{
    public static Symbol From(int value)
    {
        return new Symbol(value, Operator.Null, false);
    }

    public static Symbol From(Operator value)
    {
        return new Symbol(int.MaxValue, value, true);
    }
}

public record Expression(Symbol Root, Expression? Left, Expression? Right)
{
    public static Expression From(Operator @operator, Expression lhs, Expression rhs)
    {
        return new Expression(Symbol.From(@operator), lhs, rhs);
    }
    
    public static Expression From(Operator @operator, Expression lhs, int rhs)
    {
        return new Expression(Symbol.From(@operator), lhs, From(rhs));
    }
    
    public static Expression From(Operator @operator, int lhs, int rhs)
    {
        return new Expression(Symbol.From(@operator), From(lhs), From(rhs));
    }
    
    public static Expression From(Operator @operator)
    {
        return new Expression(Symbol.From(@operator), Left: null, Right: null);   
    }
    
    public static Expression From(int value)
    {
        return new Expression(Symbol.From(value), Left: null, Right: null);   
    }
}