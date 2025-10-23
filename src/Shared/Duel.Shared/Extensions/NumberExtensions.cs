namespace Duel.Shared.Extensions;

public static class NumberExtensions
{
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