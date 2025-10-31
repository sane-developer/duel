using Duel.Modules.Engine.Games.Muffs.Evaluation;
using Duel.Modules.Engine.Games.Muffs.Generation;
using Duel.Modules.Engine.Games.Muffs.Knowledge;
using Duel.Modules.Engine.Games.Muffs.Representation;

namespace Duel.Modules.Engine.Tests.Muffs;

[TestFixture]
public class ExpressionGeneratorTests
{
    [Test]
    public void Generate_WithDepth0_ReturnsNumber()
    {
        // Arrange
        var settings = new Difficulty
        {
            Depth = new Range(0, 0),
            Addition = new OperatorSettings(1.0, 1..10)
        };
        var generator = new ExpressionGenerator(settings, KnowledgeRegistry.Universal);
        var rng = new Random(42);

        // Act
        var expression = generator.Generate(rng);

        // Assert
        Assert.That(expression, Is.InstanceOf<Number>());
    }

    [Test]
    public void Generate_Easy_ProducesValidExpression()
    {
        // Arrange
        var generator = new ExpressionGenerator(DifficultyPresets.Easy, KnowledgeRegistry.Universal);
        var rng = new Random(42);

        // Act
        var expression = generator.Generate(rng);

        // Assert - should not throw
        var result = ExpressionEvaluator.Evaluate(expression);
        Assert.That(result, Is.TypeOf<int>());
    }

    [Test]
    public void Generate_Medium_ProducesValidExpression()
    {
        // Arrange
        var generator = new ExpressionGenerator(DifficultyPresets.Medium, KnowledgeRegistry.Universal);
        var rng = new Random(42);

        // Act
        var expression = generator.Generate(rng);

        // Assert - should not throw
        var result = ExpressionEvaluator.Evaluate(expression);
        Assert.That(result, Is.TypeOf<int>());
    }

    [Test]
    public void Generate_Hard_ProducesValidExpression()
    {
        // Arrange
        var generator = new ExpressionGenerator(DifficultyPresets.Hard, KnowledgeRegistry.Universal);
        var rng = new Random(42);

        // Act
        var expression = generator.Generate(rng);

        // Assert - should not throw
        var result = ExpressionEvaluator.Evaluate(expression);
        Assert.That(result, Is.TypeOf<int>());
    }

    [Test]
    public void Generate_MultipleExpressions_AllEvaluateSuccessfully()
    {
        // Arrange
        var generator = new ExpressionGenerator(DifficultyPresets.Easy, KnowledgeRegistry.Universal);
        var rng = new Random(42);

        // Act & Assert
        for (int i = 0; i < 100; i++)
        {
            var expression = generator.Generate(rng);
            Assert.DoesNotThrow(() => ExpressionEvaluator.Evaluate(expression));
        }
    }
}

