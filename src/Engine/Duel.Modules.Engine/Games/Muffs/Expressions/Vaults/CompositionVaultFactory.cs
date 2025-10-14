using Duel.Modules.Engine.Games.Muffs.Expressions.Symbols;

namespace Duel.Modules.Engine.Games.Muffs.Expressions.Vaults;

public static class CompositionVaultFactory
{
    public static CompositionVault Create(Range constant, params Expression.Type[] operations)
    {
        return CompositionVault
            .For(constant.Start.Value, constant.End.Value)
            .Register(operations)
            .Filter(c => c.Result is 0)
            .Filter(c => c.Result < constant.Start.Value)
            .Filter(c => c.Result > constant.End.Value)
            .Filter(c => c.Type is Expression.Type.Divide && c.Left % c.Right is not 0)
            .Compile();
    }
}

