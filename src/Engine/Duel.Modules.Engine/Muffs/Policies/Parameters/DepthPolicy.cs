namespace Duel.Modules.Engine.Muffs.Generators.Policies;

public sealed class LimitedDepthPolicy(Range limit) : IDepthPolicy
{
    public int GetDepth(Random rng)
    {
        return rng.Next(limit.Start.Value, limit.End.Value + 1);
    }
}