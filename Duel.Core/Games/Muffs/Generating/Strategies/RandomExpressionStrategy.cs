using Duel.Core.Games.Muffs.AST;

namespace Duel.Core.Games.Muffs.Generating.Strategies;

public sealed class RandomExpressionStrategy(ExpressionSettings settings, Random rng) : IExpressionStrategy
{
    public bool IsReady(int depth)
    {
        if (depth >= settings.Depth.Maximum)
        {
            return true;
        }

        var luckyThreshold = depth switch
        {
            <= 1 => 0.15,
            2 => 0.35,
            3 => 0.60,
            _ => 1.00
        };

        return rng.NextDouble() < luckyThreshold;
    }

    public Operator.Code GetOperatorCode()
    {        
        var randomOperatorWeight = rng.NextDouble() * GetTotalWeight();
        
        if ((randomOperatorWeight -= settings.Add.GetWeight()) < 0) 
        {
            return Operator.Code.Add;
        }
        
        if ((randomOperatorWeight -= settings.Subtract.GetWeight()) < 0)
        {
            return Operator.Code.Subtract;
        }

        if ((randomOperatorWeight -= settings.Multiply.GetWeight()) < 0)
        {
            return Operator.Code.Multiply;
        }
        
        if (randomOperatorWeight - settings.Divide.GetWeight() < 0)
        {
            return Operator.Code.Divide;
        }

        return Operator.Code.Power;
    }
    
    public int GetConstant() 
    {
        return rng.Next(settings.Constant.Minimum, settings.Constant.Maximum + 1);
    }

    public int GetDivisor(int dividend)
    {
        var divisor = GetRandomDivisor(dividend);
        
        if (ShouldInverseDivisor(divisor))
        {
            return -divisor;
        }
        
        return divisor;
    }

    public int GetExponent()
    {
        return rng.Next(settings.Exponent.Minimum, settings.Exponent.Maximum + 1);
    }

    private int GetRandomDivisor(int dividend)
    {
        var cursor = 0;

        var abs = Math.Abs(dividend);
        
        var limit = (int) Math.Sqrt(abs);

        const int complementaryPairsMultiplier = 2;

        Span<int> divisors = stackalloc int[limit * complementaryPairsMultiplier];
        
        for (var divisor = 1; divisor <= limit; divisor++)
        {
            if (abs % divisor != 0)
            {
                continue;
            }

            divisors[cursor++] = divisor;
            
            var complementary = abs / divisor;
            
            if (complementary != divisor)
            {
                divisors[cursor++] = complementary;
            }
        }

        var index = rng.Next(cursor);

        return divisors[index];
    }

    private bool ShouldInverseDivisor(int divisor)
    {
        var inverse = rng.NextDouble() < 0.5d;

        return IsDivisorInversionAllowed() && IsDivisorInRange(divisor) && inverse;
    }

    private bool IsDivisorInRange(int divisor)
    {
        return -divisor >= settings.Constant.Minimum;
    }

    private bool IsDivisorInversionAllowed()
    {
        return settings.Constant.Minimum < 0;
    }

    private double GetTotalWeight()
    {
        return settings.Add.GetWeight() + 
               settings.Subtract.GetWeight() + 
               settings.Multiply.GetWeight() + 
               settings.Divide.GetWeight() + 
               settings.Power.GetWeight();
    }
}