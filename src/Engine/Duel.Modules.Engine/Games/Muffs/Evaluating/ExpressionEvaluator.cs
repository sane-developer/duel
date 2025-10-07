using Duel.Modules.Engine.Games.Muffs.AST;

namespace Duel.Modules.Engine.Games.Muffs.Evaluating;

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
            Modulo node => Modulo(node),
            Power node => Power(node),
            Negation node => Negate(node),
            Abs node => Absolute(node),
            SquareRoot node => Sqrt(node),
            Factorial node => Factorial(node),
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

        int Modulo(Binary node)
        {
            return Evaluate(node.Left) % Evaluate(node.Right);
        }

        int Power(Binary node)
        {
            var lhs = Evaluate(node.Left);
            
            var rhs = Evaluate(node.Right);
            
            return (int) Math.Pow(lhs, rhs);
        }

        int Negate(Unary node)
        {
            return -Evaluate(node.Operand);
        }

        int Absolute(Unary node)
        {
            var value = Evaluate(node.Operand);

            return Math.Abs(value);
        }

        int Sqrt(Unary node)
        {
            var value = Evaluate(node.Operand);

            return (int) Math.Sqrt(value);
        }

        int Factorial(Unary node)
        {
            var value = Evaluate(node.Operand);

            if (value < 0)
            {
                throw new ArgumentException($"Factorial is undefined for negative numbers (got {value}).");
            }
        
            if (value is 0 or 1)
            {
                return 1;
            }
            
            var result = 1;
            
            for (var i = 2; i <= value; i++)
            {
                result *= i;
            }
            
            return result;
        }
    }
}