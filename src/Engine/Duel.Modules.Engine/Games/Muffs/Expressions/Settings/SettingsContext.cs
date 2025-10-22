using Duel.Modules.Engine.Games.Muffs.Expressions.AST;
using Duel.Shared.Extensions;

namespace Duel.Modules.Engine.Games.Muffs.Expressions.Settings;

internal sealed class SettingsContext(ExpressionSettings settings)
{
    public int GetDepth(Random rng)
    {
        return settings.Depth.Random(rng);
    }

    public int GetBudget(Random rng)
    {
        return settings.Budget.Random(rng);
    }

    public int GetConstant(Random rng)
    {
        return settings.Constant.Random(rng);
    }

    public int GetExponent(Random rng)
    {
        return settings.Exponent.Random(rng);
    }

    public Expression.Operator GetOperatorType(Random rng)
    {
        return settings.Operators.Random(rng);
    }
}