using Duel.Modules.Engine.Games.Muffs.Expressions.Symbols;

namespace Duel.Modules.Engine.Games.Muffs.Expressions;

public sealed class ExpressionGenerator(ExpressionCacheContext cacheContext, ExpressionSettingsContext settingsContext)
{
    private State _state;

    public Expression Generate(Random rng)
    {
        var depth = settingsContext.GetDepth(rng);

        var budget = settingsContext.GetBudget(rng);
        
        _state = new State(rng, budget);

        return GetExpression(depth);
    }

    private Expression GetExpression(int depth)
    {
        if (_state.RemainingBudget is 0 || depth is 0)
        {
            var value = settingsContext.GetConstant(_state.Rng);

            return cacheContext.GetConstant(value);
        }

        _state.RemainingBudget--;

        var operatorType = settingsContext.GetOperatorType(_state.Rng);

        var lhs = GetExpression(depth - 1);

        if (operatorType is Expression.Operator.Divide)
        {
            var dividend = ExpressionEvaluator.Evaluate(lhs);

            var divisor = cacheContext.GetDivisor(_state.Rng, dividend);

            var division = GetExpression(divisor, depth - 1);

            return Binary.From(operatorType, lhs, division);
        }

        if (operatorType is Expression.Operator.Power)
        {
            var exponent = settingsContext.GetExponent(_state.Rng);

            var power = GetExpression(exponent, depth - 1);

            return Binary.From(operatorType, lhs, power);
        }

        var rhs = GetExpression(depth - 1);

        return Binary.From(operatorType, lhs, rhs);
    }

    private Expression GetExpression(int result, int depth)
    {
        if (_state.RemainingBudget is 0 || depth is 0)
        {
            return cacheContext.GetConstant(result);
        }

        _state.RemainingBudget--;

        var composition = cacheContext.GetComposition(_state.Rng, result);
        
        var lhs = GetExpression(composition.Left, depth - 1);
        
        var rhs = GetExpression(composition.Right, depth - 1);
        
        return Binary.From(composition.Type, lhs, rhs);
    }

    private record struct State(Random Rng, int RemainingBudget);
}