namespace Duel.Shared.Ranges;

public readonly record struct IntegerRange(int Start, int End)
{
    public static implicit operator IntegerRange(Range range)
    {
        return new IntegerRange(range.Start.Value, range.End.Value);
    }

    public readonly int Random(Random rng)
    {
        return rng.Next(Start, End + 1);
    }
}