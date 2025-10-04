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
        var value = rng.NextDouble() * TotalWeight;
        
        if ((value -= AddWeight) < 0) 
        {
            return Operator.Code.Add;
        }
        
        if ((value -= SubtractWeight) < 0)
        {
            return Operator.Code.Subtract;
        }

        if ((value -= MultiplyWeight) < 0)
        {
            return Operator.Code.Multiply;
        }
        
        if (value - DivideWeight < 0)
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
        
        for (var divisor = 1; divisor <= limit; divisor++)
        {
            if (abs % divisor != 0)
            {
                continue;
            }

            divisors[cursor++] = divisor;
            
            var other = abs / divisor;
            
            if (other != divisor)
            {
                divisors[cursor++] = other;
            }
        }

        var inverse = rng.NextDouble() < 0.5d; 

        var index = rng.Next(cursor);

        var value = divisors[index];

        return inverse ? -value : value;
    }
}