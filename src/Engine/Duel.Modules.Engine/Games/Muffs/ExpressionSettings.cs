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

    public OperatorSettings Additions { get; set; }

    public OperatorSettings Subtractions { get; set; }

    public OperatorSettings Multiplications { get; set; }

    public OperatorSettings Divisions { get; set; }

    public OperatorSettings Modulos { get; set; }

    public OperatorSettings Powers { get; set; }

    public OperatorSettings Negations { get; set; }

    public OperatorSettings AbsoluteValues { get; set; }

    public OperatorSettings Factorials { get; set; }

    public OperatorSettings SquareRoots { get; set; }
}