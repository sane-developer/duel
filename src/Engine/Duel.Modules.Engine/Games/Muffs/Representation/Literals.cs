namespace Duel.Modules.Engine.Games.Muffs.Representation;

public sealed record Number(int Value) : Literal
{
    public static Number From(int value)
    {
        return new Number(value);
    }
}