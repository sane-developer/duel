namespace Duel.Shared.Extensions;

public readonly record struct NumericRange(int Start, int End)
{
    public static implicit operator NumericRange(Range range)
    {
        return new NumericRange(range.Start.Value, range.End.Value);
    }
}

public static class RangeExtensions
{
    public static int Random(this Range range, Random rng)
    {
        return rng.Next(range.Start.Value, range.End.Value + 1);
    }
}