namespace Duel.Modules.Engine.Muffs.Generators.Policies;

public sealed class LimitedOperandPolicy(int minimum, int maximum) : IOperandPolicy
{
    public int GetNumber(Random rng)
    {
        return rng.Next(minimum, maximum + 1);
    }
}