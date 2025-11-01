using Duel.Modules.Engine.Abstractions;
using Duel.Modules.Engine.Games.Muffs.Evaluation;
using Duel.Modules.Engine.Games.Muffs.Generation;
using Duel.Modules.Engine.Games.Muffs.Generation.Compositions;
using Duel.Modules.Engine.Games.Muffs.Generation.Difficulties;
using Duel.Modules.Engine.Games.Muffs.Generation.Numbers;
using Duel.Modules.Engine.Games.Muffs.Generation.Operators;
using Duel.Modules.Engine.Games.Muffs.Serialization;

namespace Duel.Modules.Engine.Games.Muffs;

/// <summary>
/// Game engine for Muffs - a mental math speed challenge.
/// Generates mathematical expressions based on difficulty and validates answers.
/// </summary>
public sealed class MuffsEngine : IEngine
{
    private readonly Dictionary<string, ExpressionGenerator> _generators;
    
    private readonly Dictionary<string, int> _timeLimits;
    
    private readonly NumbersRegistry _numbers;

    public string GameType => "Muffs";

    public MuffsEngine()
    {
        _numbers = new NumbersRegistry();

        _timeLimits = new Dictionary<string, int>
        {
            ["Easy"] = 4,
            ["Medium"] = 8,
            ["Hard"] = 12
        };

        _generators = new Dictionary<string, ExpressionGenerator>
        {
            ["Easy"] = CreateGenerator(DifficultyRegistry.Easy),
            ["Medium"] = CreateGenerator(DifficultyRegistry.Medium),
            ["Hard"] = CreateGenerator(DifficultyRegistry.Hard)
        };
    }

    public Challenge GenerateChallenge(string difficulty)
    {
        if (!_generators.TryGetValue(difficulty, out var generator))
        {
            throw new ArgumentException(
                $"Unknown difficulty: '{difficulty}'. Valid values are: Easy, Medium, Hard.",
                nameof(difficulty)
            );
        }

        if (!_timeLimits.TryGetValue(difficulty, out var timeLimit))
        {
            timeLimit = 8;
        }

        var glyph = generator.Generate(Random.Shared);

        var expression = ExpressionSerializer.Serialize(glyph);

        var correctAnswer = ExpressionEvaluator.Evaluate(glyph);

        return new Challenge(expression, correctAnswer, timeLimit);
    }

    private ExpressionGenerator CreateGenerator(Difficulty difficulty)
    {
        var compositions = new CompositionsRegistry(difficulty);
        
        var operatorSelector = new OperatorSelector(difficulty);
        
        return new ExpressionGenerator(
            difficulty,
            _numbers,
            compositions,
            operatorSelector
        );
    }
}