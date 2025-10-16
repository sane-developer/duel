namespace Duel.Modules.Engine.Games.Muffs.Expressions.Vaults;

public sealed class DivisorVault
{
    private readonly Dictionary<int, int[]> _divisorsByNumber = [];

    private DivisorVault(Range constant)
    {
        for (var number = constant.Start.Value; number <= constant.End.Value; number++)
        {
            _divisorsByNumber[number] = Compute(number);
        }
    }

    public static DivisorVault Create(Range constant)
    {
        return new DivisorVault(constant);
    }

    public int Get(Random rng, int number)
    {
        var key = Math.Abs(number);
        
        var divisors = _divisorsByNumber[key];
        
        var index = rng.Next(divisors.Length);
        
        return divisors[index];
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

