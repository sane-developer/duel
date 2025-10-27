using Duel.Modules.Engine.Games.Muffs.Evaluation;
using Duel.Modules.Engine.Games.Muffs.Generation;
using Duel.Modules.Engine.Games.Muffs.Knowledge;

namespace Duel.Modules.Engine.Tests.Muffs;

[TestFixture]
public class ExpressionSampleTests
{
    [Test]
    public void ShowEasyExpressions()
    {
        var generator = new ExpressionGenerator(DifficultyPresets.Easy, KnowledgeRegistry.Universal);
        var rng = new Random(42);

        Console.WriteLine("=== EASY Expressions ===");
        for (int i = 0; i < 10; i++)
        {
            var expr = generator.Generate(rng);
            var result = ExpressionEvaluator.Evaluate(expr);
            Console.WriteLine($"{i + 1}. Type: {expr.GetType().Name}, Result: {result}");
        }
    }

    [Test]
    public void ShowMediumExpressions()
    {
        var generator = new ExpressionGenerator(DifficultyPresets.Medium, KnowledgeRegistry.Universal);
        var rng = new Random(42);

        Console.WriteLine("=== MEDIUM Expressions ===");
        for (int i = 0; i < 10; i++)
        {
            var expr = generator.Generate(rng);
            var result = ExpressionEvaluator.Evaluate(expr);
            Console.WriteLine($"{i + 1}. Type: {expr.GetType().Name}, Result: {result}");
        }
    }

    [Test]
    public void ShowHardExpressions()
    {
        var generator = new ExpressionGenerator(DifficultyPresets.Hard, KnowledgeRegistry.Universal);
        var rng = new Random(42);

        Console.WriteLine("=== HARD Expressions ===");
        for (int i = 0; i < 10; i++)
        {
            var expr = generator.Generate(rng);
            var result = ExpressionEvaluator.Evaluate(expr);
            Console.WriteLine($"{i + 1}. Type: {expr.GetType().Name}, Result: {result}");
        }
    }
}

