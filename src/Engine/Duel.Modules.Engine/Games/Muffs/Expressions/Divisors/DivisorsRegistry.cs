using System.Collections.Frozen;

namespace Duel.Modules.Engine.Games.Muffs.Expressions.Divisors;

internal sealed class DivisorsVault
{
    private readonly FrozenDictionary<int, int[]> _vault;

    private DivisorsVault(Range constant)
    {
        var vault = new Dictionary<int, int[]>();

        for (var number = constant.Start.Value; number <= constant.End.Value; number++)
        {
            vault[number] = [.. DivisorFactory.For(number)];
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