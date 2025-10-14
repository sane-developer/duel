namespace Duel.Modules.Engine.Games.Muffs.Expressions.Vaults;

public sealed class DivisorVault
{
    private readonly Dictionary<int, int[]> _divisorsByNumber = [];

    public static DivisorVault For(int minimum, int maximum)
    {
        var vault = new DivisorVault();
        
        vault.Initialize(minimum, maximum);

        return vault;
    }

    public int GetRandomDivisor(Random rng, int number)
    {
        var divisors = _divisorsByNumber[Math.Abs(number)];
        
        var index = rng.Next(divisors.Length);
        
        return divisors[index];
    }

    private void Initialize(int minimum, int maximum)
    {
        for (var number = minimum; number <= maximum; number++)
        {
            _divisorsByNumber[number] = ComputeDivisors(number);
        }
    }

    private static int[] ComputeDivisors(int number)
    {
        var abs = Math.Abs(number);
        
        if (abs is 0)
        {
            return [];
        }

        var limit = (int) Math.Sqrt(abs);
        
        var divisors = new List<int>();
        
        for (var divisor = 1; divisor <= limit; divisor++)
        {
            if (abs % divisor is 0)
            {
                divisors.Add(divisor);
                
                var complementary = abs / divisor;
                
                if (complementary != divisor)
                {
                    divisors.Add(complementary);
                }
            }
        }

        return [.. divisors];
    }
}

