using Duel.Modules.Engine.Games.Muffs.Expressions.Symbols;

namespace Duel.Modules.Engine.Games.Muffs.Expressions.Vaults;

public sealed class ConstantVault
{
    private readonly Dictionary<int, Constant> _constants = [];

    private ConstantVault(Range range)
    {
        for (var value = range.Start.Value; value <= range.End.Value; value++)
        {
            _constants[value] = Constant.From(value);
        }
    }

    public static ConstantVault Create(Range range)
    {
        return new ConstantVault(range);
    }

    public Constant Get(int value)
    {
        return _constants[value];
    }
}

