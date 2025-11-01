using Duel.Modules.Engine.Games.Muffs.Generation.Operators;

namespace Duel.Modules.Engine.Games.Muffs.Generation.Difficulties;

public record struct Difficulty
{
    public IntegerRange Depth { get; set; }

    public IntegerRange Length { get; set; }

    public Dictionary<OperatorType, OperatorSettings> Operators { get; set; }
}