namespace Duel.Shared.Extensions;

public static class NumberExtensions
{
    /// <summary>
    /// Checks if a number is divisible by another, with zero-divisor guard.
    /// </summary>
    public static bool IsDivisibleBy(this int number, int divisor)
    {
        return divisor != 0 && number % divisor == 0;
    }

    /// <summary>
    /// Checks if a number is a perfect square (has an integer square root).
    /// </summary>
    public static bool IsPerfectSquare(this int number)
    {
        return Math.Sqrt(number) % 1 == 0;
    }

    /// <summary>
    /// Checks if a power operation will produce a result within int bounds.
    /// Prevents overflow by validating before computation.
    /// </summary>
    public static bool IsSafePower(this int @base, int exponent)
    {
        if (exponent < 0)
        {
            return false;
        }   

        if (exponent == 0)
        {
            return true;
        }
        
        if (@base is 0 or 1 or -1)
        {
            return true;
        }

        try
        {
            return Math.Pow(@base, exponent) is >= int.MinValue and <= int.MaxValue;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Calculates factorial (n!). Assumes valid input (0-12 typically).
    /// </summary>
    public static int Factorial(this int number)
    {
        var result = 1;
        
        for (var i = 2; i <= number; i++)
        {
            result *= i;
        }
        
        return result;
    }
}