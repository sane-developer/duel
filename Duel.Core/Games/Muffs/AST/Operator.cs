namespace Duel.Core.Games.Muffs.AST;

/// <summary>
///     Provides algebraic properties of operators (associativity and precedence)
///     used by parsing, serialization, and evaluation components.
/// </summary>
internal static class Operator
{
    /// <summary>
    ///     The set of supported infix operators for arithmetic expressions.
    /// </summary>
    public enum Code
    {
        Add, Subtract, Multiply, Divide, Power
    }
    
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
    ///     In this model, only <see cref="Code.Power"/> is right-associative
    ///     (e.g., <c>a ^ b ^ c</c> parses as <c>a ^ (b ^ c)</c>).
    /// </remarks>
    public static bool IsRightAssociative(Code code)
    {
        return code is Code.Power;
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
    ///     1 — <see cref="Code.Add"/>, <see cref="Code.Subtract"/><br/>
    ///     2 — <see cref="Code.Multiply"/>, <see cref="Code.Divide"/><br/>
    ///     3 — <see cref="Code.Power"/>
    /// </remarks>
    public static int Precedence(Code code)
    {
        return code switch
        {
            Code.Add or Code.Subtract => 1,
            Code.Multiply or Code.Divide => 2,
            Code.Power => 3,
            _ => 0
        };
    }
}
