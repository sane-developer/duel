namespace Duel.Shared.Ranges;

public readonly record struct IntegerRange(int Minimum, int Maximum)
{
    public static implicit operator IntegerRange(Range range)
    {
        return new IntegerRange(range.Start.Value, range.End.Value);
    }

    public readonly int Random(Random rng)
    {
        return rng.Next(Minimum, Maximum + 1);
    }
}