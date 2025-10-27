using Duel.Modules.Engine.Games.Muffs.Representation;

namespace Duel.Modules.Engine.Games.Muffs.Generation;

public record struct GeneratorSettings
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

    public readonly int GetResult(OperatorType type, Random rng)
    {
        return type switch
        {
            OperatorType.Addition => Addition.Result.Random(rng),
            OperatorType.Subtraction => Subtraction.Result.Random(rng),
            OperatorType.Multiplication => Multiplication.Result.Random(rng),
            OperatorType.Division => Division.Result.Random(rng),
            OperatorType.Modulo => Modulo.Result.Random(rng),
            OperatorType.Power => Power.Result.Random(rng),
            OperatorType.Negation => Negation.Result.Random(rng),
            OperatorType.AbsoluteValue => AbsoluteValue.Result.Random(rng),
            OperatorType.Factorial => Factorial.Result.Random(rng),
            OperatorType.SquareRoot => SquareRoot.Result.Random(rng),
            _ => Situation.Unreachable<int>()
        };
    }
}

