namespace Duel.Core.Games.Muffs.AST;

/// <summary>
///     The set of supported infix operators for arithmetic expressions.
/// </summary>
public enum OperatorCode
{
    Add, Subtract, Multiply, Divide, Power
}

/// <summary>
///     Provides algebraic properties of operators (associativity and precedence)
///     used by parsing, serialization, and evaluation components.
/// </summary>
internal static class Operator
{
    /// <summary>
    ///     Determines whether the specified operator is right-associative.
    /// </summary>
    /// <param name="code">
    ///     The operator to inspect.
    /// </param>
    /// <returns>
    ///     <c>true</c> if the operator associates to the right; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    ///     In this model, only <see cref="OperatorCode.Power"/> is right-associative
    ///     (e.g., <c>a ^ b ^ c</c> parses as <c>a ^ (b ^ c)</c>).
    /// </remarks>
    public static bool IsRightAssociative(OperatorCode code)
    {
        return code is OperatorCode.Power;
    }

    /// <summary>
    ///     Gets the precedence of the specified operator.
    /// </summary>
    /// <param name="code">
    ///     The operator to inspect.
    /// </param>
    /// <returns>
    ///     An integer where higher values bind more tightly during parsing and
    ///     parenthesis elision. For example, multiplication/division have higher
    ///     precedence than addition/subtraction.
    /// </returns>
    /// <remarks>
    ///     Current levels: <br/>
    ///     1 — <see cref="OperatorCode.Add"/>, <see cref="OperatorCode.Subtract"/><br/>
    ///     2 — <see cref="OperatorCode.Multiply"/>, <see cref="OperatorCode.Divide"/><br/>
    ///     3 — <see cref="OperatorCode.Power"/>
    /// </remarks>
    public static int Precedence(OperatorCode code)
    {
        return code switch
        {
            OperatorCode.Add or OperatorCode.Subtract => 1,
            OperatorCode.Multiply or OperatorCode.Divide => 2,
            OperatorCode.Power => 3,
            _ => 0
        };
    }
}
