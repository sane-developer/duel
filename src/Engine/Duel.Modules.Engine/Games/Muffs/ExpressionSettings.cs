namespace Duel.Modules.Engine.Games.Muffs;

public readonly record struct OperatorSettings(double Weight)
{
    public readonly bool IsAllowed => Weight > 0d;
}

public record struct ExpressionSettings
{
    public Range Depth { get; set; }

    public Range Budget { get; set; }

    public Range Number { get; set; }

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