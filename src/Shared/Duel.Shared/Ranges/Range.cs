using System.Numerics;

namespace Duel.Shared.Ranges;

public record Range<T>(T Minimum, T Maximum) where T : struct, INumber<T>
{
    public static Range<T> From(T minimum, T maximum)
    {
        return new Range<T>(minimum, maximum);
    }
}