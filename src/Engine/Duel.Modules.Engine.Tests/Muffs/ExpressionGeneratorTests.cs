using Duel.Modules.Engine.Games.Muffs.Expressions;
using Duel.Modules.Engine.Games.Muffs.Glyphs.Literals;

namespace Duel.Modules.Engine.Tests.Muffs;

[TestFixture]
public class ExpressionGeneratorTests
{
    [Test]
    public void Generate_WithDepth0_ReturnsNumber()
    {
        // Arrange
        var settings = new ExpressionSettings
        {
            Depth = new Range(0, 0),
            Addition = new OperatorSettings(1.0, 1..10)
        };
        var context = new ExpressionContext(settings);
        var generator = new ExpressionGenerator(context);
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
        var context = new ExpressionContext(ExpressionSettingsRegistry.Easy);
        var generator = new ExpressionGenerator(context);
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
        var context = new ExpressionContext(ExpressionSettingsRegistry.Medium);
        var generator = new ExpressionGenerator(context);
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
        var context = new ExpressionContext(ExpressionSettingsRegistry.Hard);
        var generator = new ExpressionGenerator(context);
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
        var context = new ExpressionContext(ExpressionSettingsRegistry.Easy);
        var generator = new ExpressionGenerator(context);
        var rng = new Random(42);

        // Act & Assert
        for (int i = 0; i < 100; i++)
        {
            var expression = generator.Generate(rng);
            Assert.DoesNotThrow(() => ExpressionEvaluator.Evaluate(expression));
        }
    }
}

