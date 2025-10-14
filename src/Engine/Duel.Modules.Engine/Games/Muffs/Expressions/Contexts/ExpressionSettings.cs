using Duel.Modules.Engine.Games.Muffs.Expressions.Symbols;

namespace Duel.Modules.Engine.Games.Muffs.Expressions.Contexts;

public sealed class ExpressionSettings
{
    public required Range Depth { get; init; }

    public required Range Constant { get; init; }

    public required Range Exponent { get; init; }

    public required Range Operators { get; init; }

    public required Expression.Type[] Operations { get; init; }
}