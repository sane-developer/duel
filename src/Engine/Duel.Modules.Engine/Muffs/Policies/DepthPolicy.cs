namespace Duel.Modules.Engine.Muffs.Policies;

public interface IDepthPolicy
{
    int GetDepth(Random rng);
}

public sealed class LimitedDepthPolicy(Range limit) : IDepthPolicy
{
    public int GetDepth(Random rng)
    {
        return rng.Next(limit.Start.Value, limit.End.Value + 1);
    }
}