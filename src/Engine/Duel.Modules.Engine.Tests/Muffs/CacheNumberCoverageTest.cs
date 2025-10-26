using Duel.Modules.Engine.Games.Muffs;
using Duel.Modules.Engine.Games.Muffs.Expressions;
using NUnit.Framework;

namespace Duel.Modules.Engine.Tests.Muffs;

[TestFixture]
public class CacheNumberCoverageTest
{
    [Test]
    public void Cache_CoversAllSingleOperationResults()
    {
        // Arrange
        var settings = ExpressionSettingsRegistry.Easy; // Numbers 1..10, +,-,*
        var cache = new MuffsCache(settings);

        // Act & Assert - should have all input numbers
        for (int i = 1; i <= 10; i++)
        {
            Assert.DoesNotThrow(() => cache.GetNumber(i), $"Missing input number {i}");
        }

        // Should have all addition results: 1+1=2 to 10+10=20
        for (int result = 2; result <= 20; result++)
        {
            Assert.DoesNotThrow(() => cache.GetNumber(result), $"Missing addition result {result}");
        }

        // Should have common multiplication results
        Assert.DoesNotThrow(() => cache.GetNumber(100), "Missing 10*10=100");
        Assert.DoesNotThrow(() => cache.GetNumber(90), "Missing 9*10=90");
    }

    [Test]
    public void Cache_HandlesNegativeNumbersFromSubtraction()
    {
        // Arrange - use Medium settings which support negative numbers
        var settings = ExpressionSettingsRegistry.Medium;
        var cache = new MuffsCache(settings);

        // Act & Assert - should have negative results from subtraction
        // Medium has operandRange -10..20, resultRange -50..100
        Assert.DoesNotThrow(() => cache.GetNumber(0), "Missing result 0");
        Assert.DoesNotThrow(() => cache.GetNumber(-30), "Missing negative result");
        Assert.DoesNotThrow(() => cache.GetNumber(-10), "Missing negative operand");
    }

    [Test]
    public void GetNumber_ThrowsForUncachedValue()
    {
        // Arrange
        var settings = new ExpressionSettings
        {
            Addition = new OperatorSettings(1.0, 1..5)
        };
        var cache = new MuffsCache(settings);

        // Act & Assert - value 1000 should not exist in cache
        Assert.Throws<KeyNotFoundException>(() => cache.GetNumber(1000));
    }
}

