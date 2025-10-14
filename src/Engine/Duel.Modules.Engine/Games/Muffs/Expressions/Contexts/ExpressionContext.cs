using Duel.Modules.Engine.Games.Muffs.Expressions.Symbols;
using Duel.Modules.Engine.Games.Muffs.Expressions.Vaults;

namespace Duel.Modules.Engine.Games.Muffs.Expressions.Contexts;

/// <summary>
///     The context for the expression generator.
/// </summary>
public sealed class ExpressionContext(Random rng, ExpressionSettings settings)
{
    /// <summary>
    ///     The random number generator.
    /// </summary>
    public Random Rng => rng;

    /// <summary>
    ///     The range of values representing the possible depth of the entire expression.
    /// </summary>
    public Range Depth => settings.Depth;

    /// <summary>
    ///     The range of values representing the possible constant values.
    /// </summary>
    public Range Constant => settings.Constant;

    /// <summary>
    ///     The range of values representing the possible exponent values.
    /// </summary>
    public Range Exponent => settings.Exponent;

    /// <summary>
    ///     The range of values representing the possible count of operators in the entire expression.
    /// </summary>
    public Range Operators => settings.Operators;

    /// <summary>
    ///     The array of supported operations.
    /// </summary>
    public Expression.Type[] Operations => settings.Operations;

    /// <summary>
    ///     The pre-computed lookup table of operand pairs that produce specific results for each binary operation.
    /// </summary>
    public readonly ExpressionVault Vault = ExpressionVaultFactory.Create(settings.Constant, settings.Operations);
}
