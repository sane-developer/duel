using Duel.Core.Games.Muffs.AST;

namespace Duel.Core.Games.Muffs.Generating.Strategies;

public sealed class RandomExpressionStrategy(Random rng) : IExpressionStrategy
{
    private const int MaximumDepth = 4;
    
    private const int MaximumExponent = 4;
    
    private const int MinimumValue = 1;
    
    private const int MaximumValue = 12;
    
    private const double AddWeight = 1.0;
    
    private const double SubtractWeight = 1.0;
    
    private const double MultiplyWeight = 0.8;
    
    private const double DivideWeight = 0.6;
    
    private const double PowerWeight = 0.2;
    
    private const double TotalWeight = AddWeight + SubtractWeight + MultiplyWeight + DivideWeight + PowerWeight;

    public bool IsReady(int depth)
    {
        if (depth >= MaximumDepth)
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
        var rand = rng.NextDouble() * TotalWeight;
        
        if ((rand -= AddWeight) < 0) 
        {
            return Operator.Code.Add;
        }
        
        if ((rand -= SubtractWeight) < 0)
        {
            return Operator.Code.Subtract;
        }

        if ((rand -= MultiplyWeight) < 0)
        {
            return Operator.Code.Multiply;
        }
        
        if (rand - DivideWeight < 0)
        {
            return Operator.Code.Divide;
        }

        return Operator.Code.Power;
    }
    
    public int GetConstant() 
    {
        return rng.Next(MinimumValue, MaximumValue + 1);
    }

    public int GetDivisor(int dividend)
    {
        return GetRandomDivisor(dividend);
    }

    public int GetExponent()
    {
        return rng.Next(MinimumValue, MaximumExponent + 1);
    }

    private int GetRandomDivisor(int dividend)
    {
        var cursor = 0;

        var abs = Math.Abs(dividend);
        
        var limit = (int) Math.Sqrt(abs);

        Span<int> divisors = stackalloc int[abs * 2];
        
        for (var i = 1; i <= limit; i++)
        {
            if (abs % i != 0)
            {
                continue;
            }

            divisors[cursor++] = i;
            
            var other = abs / i;
            
            if (other != i)
            {
                divisors[cursor++] = other;
            }
        }

        var index = rng.Next(cursor);

        var useInversion = rng.NextDouble() < 0.5d; 

        var divisor = divisors[index];

        return useInversion ? -divisor : divisor;
    }
}