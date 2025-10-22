using System.Collections.Frozen;

namespace Duel.Modules.Engine.Games.Muffs.Expressions.Vaults;

internal sealed class DivisorsVault
{
    private readonly FrozenDictionary<int, int[]> _vault;

    private DivisorsVault(Range constant)
    {
        var vault = new Dictionary<int, int[]>();

        for (var number = constant.Start.Value; number <= constant.End.Value; number++)
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

            vault[number] = [.. divisors];
        }

        _vault = vault.ToFrozenDictionary(x => x.Key, x => x.Value);
    }

    public static DivisorsVault From(Range constant)
    {
        return new DivisorsVault(constant);
    }

    public int[] For(int number)
    {
        return _vault[number];
    }
}