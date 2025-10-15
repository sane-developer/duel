namespace Duel.Modules.Engine.Games.Muffs.Expressions.Symbols;

public abstract record Expression
{
    public enum Operator
    {
        Add, Subtract, Multiply, Divide, Modulo, Power, Negate, Absolute, SquareRoot, Factorial
    }

    public readonly record struct Composition(Operator Type, int Left, int Right, int Result)
    {
        public static Composition From(Operator type, int left, int right, int result)
        {
            return new Composition(type, left, right, result);
        }
    }
}