namespace Duel.Modules.Engine.Muffs.Expressions.Parameters;

public interface ILengthParameter
{
    int GetLength(Random rng);
}

public sealed class BoundedLengthParameter(Range limit) : ILengthParameter
{
    public int GetLength(Random rng)
    {
        return rng.Next(limit.Start.Value, limit.End.Value + 1);
    }
}