namespace Duel.Modules.Engine.Games.Muffs.Expressions;

public readonly record struct OperatorSettings
{
    public double Weight { get; init; }
    
    public NumericRange Operand { get; init; }
    
    public NumericRange? Result { get; init; }
    
    public bool IsAllowed => Weight > 0d;

    public OperatorSettings(double weight, NumericRange operand, NumericRange? result = null)
    {
        Weight = weight;
        Operand = operand;
        Result = result;
    }
}

public record struct ExpressionSettings
{
    public Range Depth { get; set; }

    public Range Length { get; set; }

    public OperatorSettings Addition { get; set; }

    public OperatorSettings Subtraction { get; set; }

    public OperatorSettings Multiplication { get; set; }

    public OperatorSettings Division { get; set; }

    public OperatorSettings Modulo { get; set; }

    public OperatorSettings Power { get; set; }

    public OperatorSettings Negation { get; set; }

    public OperatorSettings AbsoluteValue { get; set; }

    public OperatorSettings Factorial { get; set; }

    public OperatorSettings SquareRoot { get; set; }
}