using Duel.Core.Games.Muffs.AST;
using Duel.Core.Shared;

namespace Duel.Core.Games.Muffs.Generating;

public sealed class ExpressionGenerator(Random rng)
{
    private const int DefaultDepth = 1;

    private const int MaximumDepth = 4;

    private const int MaximumExponent = 4;
    
    private const int MinimumComponentValue = 1;

    private const int MaximumComponentValue = 12;

    private const double AddOperatorWeight = 1.0;
    
    private const double SubtractOperatorWeight = 1.0;
    
    private const double MultiplyOperatorWeight = 0.8;
    
    private const double DivideOperatorWeight = 0.6;
    
    private const double PowerOperatorWeight = 0.2;
    
    private const double TotalWeightValue = 
        AddOperatorWeight + 
        SubtractOperatorWeight + 
        MultiplyOperatorWeight + 
        DivideOperatorWeight + 
        PowerOperatorWeight;
    
    public Expression Generate()
    {
        return GetRandomNode(DefaultDepth);
    }

    private Expression GetRandomNode(int depth)
    {
        var hasExceededMaxDepth = depth >= MaximumDepth;
        
        var hasDrawnLuckyNumber = rng.NextDouble() < GetLuckyNumber(depth);

        if (hasExceededMaxDepth || hasDrawnLuckyNumber)
        {
            return GetRandomConstant();
        }

        var code = GetRandomOperatorCode();

        var lhs = GetRandomNode(depth + 1);
        
        var rhs = GetRightOperand(code, depth + 1);
        
        return code switch
        {
            Operator.Code.Add => Addition.From(lhs, rhs),
            Operator.Code.Subtract => Subtraction.From(lhs, rhs),
            Operator.Code.Multiply => Multiplication.From(lhs, rhs),
            Operator.Code.Divide => Division.From(lhs, rhs),
            Operator.Code.Power => Power.From(lhs, rhs),
            _ => Situation.Unreachable<Expression>()
        };
    }

    private Operator.Code GetRandomOperatorCode()
    {
        var randomOperatorWeight = rng.NextDouble() * TotalWeightValue;

        if ((randomOperatorWeight -= AddOperatorWeight) < 0)
        {
            return Operator.Code.Add;
        }

        if ((randomOperatorWeight -= SubtractOperatorWeight) < 0)
        {
            return Operator.Code.Subtract;
        }

        if ((randomOperatorWeight -= MultiplyOperatorWeight) < 0)
        {
            return Operator.Code.Multiply;
        }

        return randomOperatorWeight - DivideOperatorWeight < 0 
            ? Operator.Code.Divide 
            : Operator.Code.Power;
    }
    
    private Expression GetRightOperand(Operator.Code op, int depth)
    {
        return op switch
        {
            Operator.Code.Divide => GetDivisorConstant(),
            Operator.Code.Power => GetExponentConstant(),
            _ => GetRandomNode(depth)
        };
    }

    private Constant GetDivisorConstant()
    {
        var dividend = GetRandomNumber();

        var divisors = GetNumberDivisors(dividend);
        
        var divisor = GetRestrictedRandomNumber(in divisors);
        
        return Constant.From(divisor);
    }
    
    private Constant GetExponentConstant()
    {
        var value = GetRestrictedRandomNumber(MaximumExponent);
        
        return Constant.From(value);
    }
    
    private Constant GetRandomConstant()
    {
        var value = GetRandomNumber();

        return Constant.From(value);
    }

    private int GetRestrictedRandomNumber(in List<int> numbers)
    {
        var index = rng.Next(numbers.Count);
        
        return numbers[index];
    }
    
    private int GetRestrictedRandomNumber(int limit)
    {
        return rng.Next(MinimumComponentValue, limit + 1);
    }
    
    private int GetRandomNumber()
    {
        return rng.Next(MinimumComponentValue, MaximumComponentValue + 1);
    }
    
    private static double GetLuckyNumber(int depth)
    {
        return depth switch
        {
            <= 1 => 0.15,
            2 => 0.35,
            3 => 0.60,
            _ => 1.00
        };
    }
    
    private static List<int> GetNumberDivisors(int dividend)
    {
        var divisors = new List<int>();
        
        var abs = Math.Abs(dividend);
        
        var limit = (int) Math.Sqrt(abs);

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

        var count = divisors.Count;
        
        for (var j = 0; j < count; j++)
        {
            var divisor = -divisors[j];
            
            divisors.Add(divisor);
        }

        return divisors;
    }
}