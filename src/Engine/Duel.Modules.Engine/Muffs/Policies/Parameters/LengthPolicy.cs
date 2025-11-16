namespace Duel.Modules.Engine.Muffs.Policies.Parameters;

public interface ILengthPolicy
{
    int GetLength(Random rng);
}

public sealed class LimitedLengthPolicy(Range limit) : ILengthPolicy
{
    public int GetLength(Random rng)
    {
        return rng.Next(limit.Start.Value, limit.End.Value + 1);
    }
}