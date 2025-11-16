namespace Duel.Modules.Engine.Muffs.Policies;

public interface IOperandPolicy
{
    int GetNumber(Random rng);
}

public sealed class LimitedOperandPolicy(int minimum, int maximum) : IOperandPolicy
{
    public int GetNumber(Random rng)
    {
        return rng.Next(minimum, maximum + 1);
    }
}