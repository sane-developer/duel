namespace Duel.Modules.Engine.Games.Muffs.Expressions.Divisors;

public static class DivisorFactory
{
    public static IEnumerable<int> For(int number)
    {
        var abs = Math.Abs(number);

        var limit = (int) Math.Sqrt(abs);
        
        for (var divisor = 1; divisor <= limit; divisor++)
        {
            if (abs % divisor is not 0)
            {
                continue;
            }
            
            yield return divisor;

            var complementary = abs / divisor;

            if (complementary != divisor)
            {
                yield return complementary;
            }
        }
    }
}