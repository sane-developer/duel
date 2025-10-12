using Duel.Modules.Engine.Games.Muffs.Expressions.Symbols;

namespace Duel.Modules.Engine.Games.Muffs.Expressions;

public static class ExpressionEvaluator
{
    public static int Evaluate(Expression expression)
    {
        return expression switch
        {
            Constant node => node.Value,
            Addition node => Add(node),
            Subtraction node => Subtract(node),
            Multiplication node => Multiply(node),
            Division node => Divide(node),
            Modulo node => Modulo(node),
            Power node => Power(node),
            Negation node => Negate(node),
            Absolute node => Absolute(node),
            SquareRoot node => SquareRoot(node),
            Factorial node => Factorial(node),
            _ => Situation.Unreachable<int>()
        };
    }

    private static int Add(Binary node)
    {
        return Evaluate(node.Left) + Evaluate(node.Right);
    }

    private static int Subtract(Binary node)
    {
        return Evaluate(node.Left) - Evaluate(node.Right);
    }

    private static int Multiply(Binary node)
    {
        return Evaluate(node.Left) * Evaluate(node.Right);
    }

    private static int Divide(Binary node)
    {
        return Evaluate(node.Left) / Evaluate(node.Right);
    }

    private static int Modulo(Binary node)
    {
        return Evaluate(node.Left) % Evaluate(node.Right);
    }

    private static int Power(Binary node)
    {
        var lhs = Evaluate(node.Left);
        
        var rhs = Evaluate(node.Right);
        
        return (int) Math.Pow(lhs, rhs);
    }

    private static int Negate(Unary node)
    {
        return -Evaluate(node.Operand);
    }

    private static int Absolute(Unary node)
    {
        var value = Evaluate(node.Operand);

        return Math.Abs(value);
    }

    private static int SquareRoot(Unary node)
    {
        var value = Evaluate(node.Operand);

        return (int) Math.Sqrt(value);
    }

    private static int Factorial(Unary node)
    {
        var value = Evaluate(node.Operand);

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