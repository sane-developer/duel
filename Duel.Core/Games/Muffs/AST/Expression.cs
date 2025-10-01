namespace Duel.Core.Games.Muffs.AST;

/// <summary>
///     Represents a node in the arithmetic expression abstract syntax tree (AST).
///     This is the base type for all expression nodes used by the Muffs game.
/// </summary>
public abstract record Expression;

/// <summary>
///     A literal integer value within an expression.
/// </summary>
/// <param name="Value">
///     The integer value of the literal.
/// </param>
public sealed record Constant(int Value) : Expression
{
    public static Constant From(int value)
    {
        return new Constant(value);
    }
}

/// <summary>
///     A binary operator application with a left and right operand.
///     Serves as the base type for concrete operator nodes.
/// </summary>
/// <param name="Left">
///     The left-hand side operand. Must not be <c>null</c>.
/// </param>
/// <param name="Right">
///     The right-hand side operand. Must not be <c>null</c>.
/// </param>
public abstract record Binary(Expression Left, Expression Right) : Expression;

/// <summary>
///     Represents an addition operation (<c>a + b</c>).
/// </summary>
/// <param name="Left">
///     The left-hand side operand.
/// </param>
/// <param name="Right">
///     The right-hand side operand.
/// </param>
public sealed record Addition(Expression Left, Expression Right) : Binary(Left, Right)
{
    public static Addition From(Expression left, Expression right)
    {
        return new Addition(left, right);
    }
}

/// <summary>
///     Represents a subtraction operation (<c>a - b</c>).
/// </summary>
/// <param name="Left">
///     The left-hand side operand.
/// </param>
/// <param name="Right">
///     The right-hand side operand.
/// </param>
public sealed record Subtraction(Expression Left, Expression Right) : Binary(Left, Right)
{
    public static Subtraction From(Expression left, Expression right)
    {
        return new Subtraction(left, right);
    }   
}

/// <summary>
///     Represents a multiplication operation (<c>a * b</c>).
/// </summary>
/// <param name="Left">
///     The left-hand side operand.
/// </param>
/// <param name="Right">
///     The right-hand side operand.
/// </param>
public sealed record Multiplication(Expression Left, Expression Right) : Binary(Left, Right)
{
    public static Multiplication From(Expression left, Expression right)
    {
        return new Multiplication(left, right);
    }   
}

/// <summary>
///     Represents a division operation (<c>a / b</c>).
/// </summary>
/// <param name="Left">
///     The left-hand side operand.
/// </param>
/// <param name="Right">
///     The right-hand side operand.
/// </param>
/// <remarks>
///     Division is integer division. Division by zero is not allowed and
///     should be detected during evaluation.
/// </remarks>
public sealed record Division(Expression Left, Expression Right) : Binary(Left, Right)
{
    public static Division From(Expression left, Expression right)
    {
        return new Division(left, right);
    }   
}

/// <summary>
///     Represents an exponentiation operation (<c>a ^ b</c>).
/// </summary>
/// <param name="Left">
///     The base value.
/// </param>
/// <param name="Right">
///     The exponent. Must be non-negative in valid expressions.
/// </param>
/// <remarks>
///     Exponentiation is right-associative. For example, <c>2 ^ 3 ^ 2</c>
///     is parsed as <c>2 ^ (3 ^ 2)</c>.
/// </remarks>
public sealed record Power(Expression Left, Expression Right) : Binary(Left, Right)
{
    public static Power From(Expression left, Expression right)
    {
        return new Power(left, right);
    }
}
