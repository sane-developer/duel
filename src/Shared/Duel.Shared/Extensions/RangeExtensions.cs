namespace Duel.Shared.Extensions;

public static class RangeExtensions
{
    public static int Random(this Range range, Random rng)
    {
        return rng.Next(range.Start.Value, range.End.Value + 1);
    }
}