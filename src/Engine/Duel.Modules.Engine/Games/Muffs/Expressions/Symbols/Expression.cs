namespace Duel.Modules.Engine.Games.Muffs.Expressions.Symbols;

public abstract record Expression
{
    public enum Type
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

    public readonly record struct Composition(Type Type, int Left, int Right, int Result)
    {
        public static Composition From(Type type, int left, int right, int result)
        {
            return new Composition(type, left, right, result);
        }
    }

    public T As<T>() where T : Expression
    {
        return (T) this;
    }
}