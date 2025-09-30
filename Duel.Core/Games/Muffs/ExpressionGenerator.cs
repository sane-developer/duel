namespace Duel.Core.Games.Muffs;

public sealed class ExpressionGenerator(Random rng)
{
    private const int Attempts = 1000;
    
    private const int OneHundredPercent = 100;
   
    private const int LuckyNumberThreshold = 90;

    private static readonly Dictionary<OperatorType, Operator> Operators = new()
    {
        [OperatorType.Add] = Operator.Add,
        [OperatorType.Subtract] = Operator.Subtract,
        [OperatorType.Multiply] = Operator.Multiply,
        [OperatorType.Divide] = Operator.Divide,
        [OperatorType.Power] = Operator.Power
    };
    
    private static readonly List<OperatorType> NonLeafOperatorTypes = 
    [
        OperatorType.Add,
        OperatorType.Subtract,
        OperatorType.Multiply
    ];

    public Expression Generate(ExpressionPolicy policy)
    {
        for (var i = 0; i < Attempts; i++)
        {
            var depth = GetRandomNumber(policy.MaxDepth);

            if (depth is 1)
            {
                return BuildLeafLevel(policy);
            }

            var expression = BuildComposite(policy, depth);
            
            var result = ExpressionEvaluator.Evaluate(expression);

            if (result >= policy.MaxAbsoluteValue)
            {
                continue;
            }

            return expression;
        }

        throw new UnableToGenerateExpressionException("Too restrictive policy for expression generation.");
    }

    private Expression BuildComposite(ExpressionPolicy policy, int depth)
    {
        if (depth <= 1)
        {
            return BuildLeafLevel(policy);
        }

        var type = GetOperatorTypeNonLeaf();
        
        var op = Operators[type];

        var left = BuildComposite(policy, depth - 1);
       
        var right = BuildComposite(policy, depth - 1);

        return Expression.From(op, left, right);
    }

    private Expression BuildLeafLevel(ExpressionPolicy policy)
    {
        var luckyNumber = GetRandomNumber(OneHundredPercent);
        
        if (luckyNumber >= LuckyNumberThreshold)
        {
            return Expression.From(luckyNumber);
        }

        var type = GetOperatorTypeLeaf();

        if (type is OperatorType.Power)
        {
            return GetPowerExpression(policy);
        }

        if (type is OperatorType.Multiply)
        {
            return GetMultiplierExpression(policy);
        }

        if (type is OperatorType.Divide)
        {
            return GetDivisionExpression(policy);
        }

        return GetBasicExpression(policy, type);
    }

    private Expression GetBasicExpression(ExpressionPolicy policy, OperatorType type)
    {
        var op = Operators[type];
        
        var lhs = GetNumberExpression(policy.MaxComponentValue);
        
        var rhs = GetNumberExpression(policy.MaxComponentValue);
        
        return Expression.From(op, lhs, rhs);
    }

    private Expression GetPowerExpression(ExpressionPolicy policy)
    {
        var lhs = GetNumberExpression(policy.MaxExponentBaseValue);
        
        var rhs = GetNumberExpression(policy.MaxExponentValue);
        
        return Expression.From(Operator.Power, lhs, rhs);
    }

    private Expression GetMultiplierExpression(ExpressionPolicy policy)
    {
        var lhs = GetNumberExpression(policy.MaxMultiplierValue);
        
        var rhs = GetNumberExpression(policy.MaxMultiplierValue);
        
        return Expression.From(Operator.Multiply, lhs, rhs);
    }

    private Expression GetDivisionExpression(ExpressionPolicy policy)
    {
        var dividend = GetRandomNumber(policy.MaxDividendValue);
        
        var divisors = GetNumberDivisors(dividend);

        var lhs = Expression.From(dividend);
        
        var rhs = GetNumberExpression(divisors);
        
        return Expression.From(Operator.Divide, lhs, rhs);
    }

    private Expression GetNumberExpression(int maxValue)
    {
        var number = GetRandomNumber(maxValue);
        
        return Expression.From(number);
    }

    private Expression GetNumberExpression(List<int> numbers)
    {
        var index = rng.Next(numbers.Count);
        
        var number = numbers[index];
        
        return Expression.From(number);
    }

    private OperatorType GetOperatorTypeLeaf()
    {
        const int min = (int) OperatorType.Add;
        
        const int max = (int) OperatorType.Power + 1;
        
        return (OperatorType) rng.Next(min, max);
    }

    private OperatorType GetOperatorTypeNonLeaf()
    {
        var index = rng.Next(NonLeafOperatorTypes.Count);
        
        return NonLeafOperatorTypes[index];
    }

    private int GetRandomNumber(int maxValue)
    {
        const int min = 1;
        
        var max = maxValue + 1;
        
        return rng.Next(min, max);
    }

    private static List<int> GetNumberDivisors(int dividend)
    {
        var divisors = new List<int>();
        
        var abs = Math.Abs(dividend);
        
        var limit = (int)Math.Sqrt(abs);

        for (var i = 1; i <= limit; i++)
        {
            if (abs % i != 0)
            {
                continue;
            }

            divisors.Add(i);
            
            var other = abs / i;
            
            if (other != i)
            {
                divisors.Add(other);
            }
        }

        if (dividend >= 0)
        {
            return divisors;
        }

        for (var j = 0; j < divisors.Count; j++)
        {
            var divisor = -divisors[j];
            
            divisors.Add(divisor);
        }

        return divisors;
    }
}

public class UnableToGenerateExpressionException(string message) : Exception(message);
