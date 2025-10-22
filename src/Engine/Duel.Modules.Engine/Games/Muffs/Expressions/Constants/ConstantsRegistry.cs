using Duel.Modules.Engine.Games.Muffs.Expressions.AST;
using System.Collections.Frozen;

namespace Duel.Modules.Engine.Games.Muffs.Expressions.Constants;

internal sealed class ConstantsVault
{
    private readonly FrozenDictionary<int, Constant> _vault;
    
    private ConstantsVault(Range constant)
    {
        var vault = new Dictionary<int, Constant>();

        for (var number = constant.Start.Value; number <= constant.End.Value; number++)
        {
            vault[number] = Constant.From(number);
        }

        _vault = vault.ToFrozenDictionary(x => x.Key, x => x.Value);
    }

    public static ConstantsVault From(Range constant)
    {
        return new ConstantsVault(constant);
    }

    public Constant For(int number)
    {
        return _vault[number];
    }
}