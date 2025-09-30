using System.Diagnostics;
using Duel.Core.Games.Muffs.AST;

namespace Duel.Core.Games.Muffs.Evaluating;

/// <summary>
///     Evaluates arithmetic <see cref="Expression"/> trees to a 32-bit integer result.
///     Uses explicit operator node types (<see cref="Addition"/>, <see cref="Division"/>, etc.).
/// </summary>
public static class ExpressionEvaluator
{
    /// <summary>
    ///     Computes the integer value of given expression.
    /// </summary>
    /// <param name="expression">
    ///     Server-side generated <see cref="Expression"/> to be evaluated.
    /// </param>
    public static int Evaluate(Expression expression)
    {
        return expression switch
        {
            Constant node => node.Value,
            Addition node => Add(node),
            Subtraction node => Subtract(node),
            Multiplication node => Multiply(node),
            Division node => Divide(node),
            Power node => Power(node),
            _ => Unreachable()
        };

        int Add(Binary node)
        {
            return Evaluate(node.Left) + Evaluate(node.Right);
        }

        int Subtract(Binary node)
        {
            return Evaluate(node.Left) - Evaluate(node.Right);
        }

        int Multiply(Binary node)
        {
            return Evaluate(node.Left) * Evaluate(node.Right);
        }

        int Divide(Binary node)
        {
            return Evaluate(node.Left) / Evaluate(node.Right);
        }

        int Power(Binary node)
        {
            var lhs = Evaluate(node.Left);
            
            var rhs = Evaluate(node.Right);
            
            return (int) Math.Pow(lhs, rhs);
        }

        int Unreachable()
        {
            throw new UnreachableException();
        }
    }
}