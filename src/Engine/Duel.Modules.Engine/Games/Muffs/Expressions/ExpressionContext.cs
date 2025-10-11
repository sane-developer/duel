using Duel.Shared.Ranges;

namespace Duel.Modules.Engine.Games.Muffs.Expressions;

/// <summary>
///     The context for the expression generator.
/// </summary>
public interface IExpressionContext
{
    /// <summary>
    ///     The random number generator.
    /// </summary>
    Random Rng { get; }

    /// <summary>
    ///     The range of values representing the possible depth of the entire expression.
    /// </summary>
    Range<int> Depth { get; }

    /// <summary>
    ///     The range of values representing the possible constant values.
    /// </summary>
    Range<int> Constant { get; }

    /// <summary>
    ///     The range of values representing the possible exponent values.
    /// </summary>
    Range<int> Exponent { get; }

    /// <summary>
    ///     The range of values representing the possible count of operators in the entire expression.
    /// </summary>
    Range<int> Operators { get; }

    /// <summary>
    ///     The pre-computed lookup table of operand pairs that produce specific results for each binary operation.
    /// </summary>
    ExpressionVault Vault { get; }
}