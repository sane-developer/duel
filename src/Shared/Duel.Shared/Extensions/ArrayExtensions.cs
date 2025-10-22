namespace Duel.Shared.Extensions;

public static class ArrayExtensions
{
    public static T Random<T>(this T[] array, Random rng)
    {
        var index = rng.Next(array.Length);

        return array[index];
    }
}