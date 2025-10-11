namespace Duel.Modules.Engine.Games.Muffs.Expressions.Symbols;

public sealed record Constant(int Value) : Expression
{
    public static Constant From(int value)
    {
        return new Constant(value);
    }
}