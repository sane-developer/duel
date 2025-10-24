using Duel.Modules.Engine.Games.Muffs;
using NUnit.Framework;

namespace Duel.Modules.Engine.Tests.Muffs;

[TestFixture]
public class ExpressionSampleTests
{
    [Test]
    public void ShowEasyExpressions()
    {
        var context = new ExpressionContext(ExpressionSettingsRegistry.Easy);
        var generator = new ExpressionGenerator(context);
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
        var context = new ExpressionContext(ExpressionSettingsRegistry.Medium);
        var generator = new ExpressionGenerator(context);
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
        var context = new ExpressionContext(ExpressionSettingsRegistry.Hard);
        var generator = new ExpressionGenerator(context);
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

