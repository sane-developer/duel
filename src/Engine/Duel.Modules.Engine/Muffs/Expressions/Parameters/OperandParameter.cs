namespace Duel.Modules.Engine.Muffs.Expressions.Parameters;

public interface IOperandParameter
{
    int GetNumber(Random rng);
}

public sealed class BoundedOperandParameter(int minimum, int maximum) : IOperandParameter
{
    public int GetNumber(Random rng)
    {
        return rng.Next(minimum, maximum + 1);
    }
}