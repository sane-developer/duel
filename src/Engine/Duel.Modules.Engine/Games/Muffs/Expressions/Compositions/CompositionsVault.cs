using System.Collections.Frozen;

namespace Duel.Modules.Engine.Games.Muffs.Expressions.Compositions;

internal sealed class CompositionsVault
{
    private readonly FrozenDictionary<int, Composition[]> _vault;
    
    private CompositionsVault(List<Composition> compositions)
    {
        _vault = compositions.GroupBy(x => x.Result).ToFrozenDictionary(x => x.Key, x => x.ToArray());
    }

    public static CompositionsVault From(List<Composition> compositions)
    {
        return new CompositionsVault(compositions);
    }

    public Composition[] For(int number)
    {
        return _vault[number];
    }
}