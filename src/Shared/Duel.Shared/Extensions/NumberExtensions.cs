namespace Duel.Shared.Extensions;

public static class NumberExtensions
{
    public static bool IsPerfectSquare(this int number)
    {
        return Math.Sqrt(number) % 1 is 0;
    }

    public static bool IsSafePower(this int @base, int exponent)
    {
        if (exponent < 0)
        {
            return false;
        }

        if (exponent is 0)
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

    public static int Power(this int @base, int exponent)
    {
        return (int) Math.Pow(@base, exponent);
    }

    public static int Negate(this int number)
    {
        return -number;
    }

    public static int Absolute(this int number)
    {
        return Math.Abs(number);
    }

    public static int SquareRoot(this int number)
    {
        return (int) Math.Sqrt(number);
    }

    public static int Factorial(this int number)
    {
        var result = 1;

        for (var i = 2; i <= number; i++)
        {
            result *= i;
        }

        return result;
    }

    public static IEnumerable<int> Divisors(this int number)
    {
        var abs = Math.Abs(number);

        var limit = (int) Math.Sqrt(abs);

        for (var i = 1; i <= limit; i++)
        {
            if (abs % i is 0)
            {
                yield return i;

                if (i != abs / i)
                {    
                    yield return abs / i;
                }
            }
        }
    }
}