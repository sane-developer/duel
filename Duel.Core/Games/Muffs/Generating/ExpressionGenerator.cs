using Duel.Core.Games.Muffs.AST;
using Duel.Core.Shared;

namespace Duel.Core.Games.Muffs.Generating;

public static class ExpressionGenerator
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
    
    public static Expression Generate(Random rng)
    {
        return GetRandomNode(DefaultDepth, rng);
    }

    private static Expression GetRandomNode(int depth, Random rng)
    {
        var hasExceededMaxDepth = depth >= MaximumDepth;
        
        var hasDrawnLuckyNumber = rng.NextDouble() < GetLuckyNumber(depth);

        if (hasExceededMaxDepth || hasDrawnLuckyNumber)
        {
            return GetRandomConstant(rng);
        }

        var code = GetRandomOperatorCode(rng);

        var lhs = GetRandomNode(depth + 1, rng);
        
        var rhs = GetRightOperand(code, depth + 1, rng);
        
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

    private static Operator.Code GetRandomOperatorCode(Random rng)
    {
        var r = rng.NextDouble() * TotalWeightValue;

        if ((r -= AddOperatorWeight) < 0)
        {
            return Operator.Code.Add;
        }

        if ((r -= SubtractOperatorWeight) < 0)
        {
            return Operator.Code.Subtract;
        }

        if ((r -= MultiplyOperatorWeight) < 0)
        {
            return Operator.Code.Multiply;
        }

        return r - DivideOperatorWeight < 0 
            ? Operator.Code.Divide 
            : Operator.Code.Power;
    }
    
    private static Expression GetRightOperand(Operator.Code op, int depth, Random rng)
    {
        return op switch
        {
            Operator.Code.Divide => GetDivisorConstant(rng),
            Operator.Code.Power => GetExponentConstant(rng),
            _ => GetRandomNode(depth, rng)
        };
    }

    private static Constant GetDivisorConstant(Random rng)
    {
        var dividend = GetRandomNumber(rng);

        var divisors = GetNumberDivisors(dividend);
        
        var divisor = GetRestrictedRandomNumber(rng, in divisors);
        
        return Constant.From(divisor);
    }
    
    private static Constant GetExponentConstant(Random rng)
    {
        var value = GetRestrictedRandomNumber(rng, MaximumExponent);
        
        return Constant.From(value);
    }
    
    private static Constant GetRandomConstant(Random rng)
    {
        var value = GetRandomNumber(rng);

        return Constant.From(value);
    }

    private static int GetRestrictedRandomNumber(Random rng, in List<int> numbers)
    {
        var index = rng.Next(numbers.Count);
        
        return numbers[index];
    }
    
    private static int GetRestrictedRandomNumber(Random rng, int limit)
    {
        return rng.Next(MinimumComponentValue, limit + 1);
    }
    
    private static int GetRandomNumber(Random rng)
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