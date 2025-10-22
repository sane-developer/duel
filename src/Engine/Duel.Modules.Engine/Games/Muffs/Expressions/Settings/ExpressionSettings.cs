using Duel.Modules.Engine.Games.Muffs.Expressions.Symbols;

namespace Duel.Modules.Engine.Games.Muffs.Expressions.Settings;

public record struct ExpressionSettings
{
    public Range Depth { get; set; }

    public Range Budget { get; set; }

    public Range Constant { get; set; }

    public Range Exponent { get; set; }

    public Expression.Operator[] Operators { get; set; }

    public ExpressionSettings()
    {
        Operators = [];
    }
}