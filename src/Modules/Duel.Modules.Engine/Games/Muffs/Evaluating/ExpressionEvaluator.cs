using Duel.Core.Games.Muffs.AST;
using Duel.Shared.Common;

namespace Duel.Core.Games.Muffs.Evaluating;

public sealed class ExpressionEvaluator(Expression expression)
{
    public int Evaluate()
    {
        return Evaluate(expression);
    }

    private static int Evaluate(Expression root)
    {
        return root switch
        {
            Constant node => node.Value,
            Addition node => Add(node),
            Subtraction node => Subtract(node),
            Multiplication node => Multiply(node),
            Division node => Divide(node),
            Power node => Power(node),
            _ => Situation.Unreachable<int>()
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
    }
}