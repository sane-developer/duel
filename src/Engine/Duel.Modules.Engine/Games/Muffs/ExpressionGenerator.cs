using Duel.Modules.Engine.Games.Muffs.AST;
using Duel.Modules.Engine.Games.Muffs.AST.Operators;
using Duel.Shared.Extensions;

namespace Duel.Modules.Engine.Games.Muffs;

public record struct ExpressionState(Random Rng, int Budget)
{
    private int _budget = Budget;

    public void ReduceBudget()
    {
        _budget -= 1;
    }

    public readonly bool IsBudgetExhausted()
    {
        return _budget is 0;
    }
};

public sealed class ExpressionGenerator(ExpressionContext context)
{
    private ExpressionState _state;

    public Glyph Generate(Random rng)
    {
        var depth = context.GetDepth(rng);

        _state = new ExpressionState(rng, context.GetBudget(rng));

        return GenerateRandomExpression(depth);
    }

    private Glyph GenerateRandomExpression(int depth)
    {
        if (depth == 0 || _state.IsBudgetExhausted())
        {
            var number = context.GetNumber(_state.Rng);
            
            return context.GetNumber(number);
        }

        _state.ReduceBudget();

        var operatorType = context.GetOperatorType(_state.Rng);

        var lhs = GenerateRandomExpression(depth - 1);

        if (operatorType is OperatorType.Division)
        {
            var dividend = ExpressionEvaluator.Evaluate(lhs);

            var divisors = context.GetDivisors(dividend);
            
            var divisor = divisors.Random(_state.Rng);

            return GenerateSpecificExpression(divisor, depth - 1);
        }

        var rhs = GenerateRandomExpression(depth - 1);

        return operatorType switch
        {
            OperatorType.Addition => new Add(lhs, rhs),
            OperatorType.Subtraction => new Subtract(lhs, rhs),
            OperatorType.Multiplication => new Multiply(lhs, rhs),
            OperatorType.Modulo => new Modulo(lhs, rhs),
            OperatorType.Power => new Power(lhs, rhs),
            OperatorType.Negation => new Negate(lhs),
            OperatorType.AbsoluteValue => new Absolute(lhs),
            OperatorType.Factorial => new Factorial(lhs),
            OperatorType.SquareRoot => new SquareRoot(lhs),
            _ => Situation.Unreachable<Glyph>()
        };
    }
    
    private Glyph GenerateSpecificExpression(int number, int depth)
    {
        return null!;
    }

    private int GetLeftBudget()
    {
        return (int) Math.Floor(_state.Budget / 2d);
    }

    private int GetRightBudget(int leftBudget)
    {
        return _state.Budget - leftBudget;
    }
}