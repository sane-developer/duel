namespace Duel.Modules.Engine.Muffs.Expressions.Parameters;

public interface IDepthParameter
{
    int GetDepth(Random rng);
}

public sealed class BoundedDepthParameter(Range limit) : IDepthParameter
{
    public int GetDepth(Random rng)
    {
        return rng.Next(limit.Start.Value, limit.End.Value + 1);
    }
}