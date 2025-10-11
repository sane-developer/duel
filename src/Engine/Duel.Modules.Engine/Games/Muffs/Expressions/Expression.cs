namespace Duel.Modules.Engine.Games.Muffs.Expressions;

public abstract record Expression
{
    public T As<T>() where T : Expression
    {
        return (T) this;
    }
}

public enum ExpressionType
{
    Add, 
    Subtract, 
    Multiply, 
    Divide, 
    Modulo, 
    Power,
    Negate, 
    Absolute, 
    SquareRoot, 
    Factorial
}