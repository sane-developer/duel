namespace Duel.Modules.Engine.Games.Muffs.Expressions.AST;

public abstract record Expression
{
    public enum Operator
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
}