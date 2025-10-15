namespace Duel.Modules.Engine.Games.Muffs.Expressions.Vaults;

public sealed class DivisorVault
{
    private readonly Dictionary<int, int[]> _divisorsByNumber = [];

    public static DivisorVault For(Range constant)
    {
        var vault = new DivisorVault();
        
        vault.Initialize(constant.Start.Value, constant.End.Value);

        return vault;
    }

    public int GetRandom(int number, Random rng)
    {
        var key = Math.Abs(number);
        
        var divisors = _divisorsByNumber[key];
        
        var index = rng.Next(divisors.Length);
        
        return divisors[index];
    }

    private void Initialize(int minimum, int maximum)
    {
        for (var number = minimum; number <= maximum; number++)
        {
            _divisorsByNumber[number] = Compute(number);
        }
    }

    private static int[] Compute(int number)
    {
        var abs = Math.Abs(number);
        
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

