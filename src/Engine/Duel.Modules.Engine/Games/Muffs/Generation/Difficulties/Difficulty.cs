using Duel.Modules.Engine.Games.Muffs.Generation.Operators;

namespace Duel.Modules.Engine.Games.Muffs.Generation.Difficulties;

public record struct Difficulty
{
    public Range Depth { get; set; }

    public Range Length { get; set; }

    public Dictionary<OperatorType, OperatorSettings> Operators { get; set; }
}