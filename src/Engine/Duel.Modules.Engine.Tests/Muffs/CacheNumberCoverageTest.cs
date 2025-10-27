using Duel.Modules.Engine.Games.Muffs.Generation;
using Duel.Modules.Engine.Games.Muffs.Knowledge;

namespace Duel.Modules.Engine.Tests.Muffs;

[TestFixture]
public class CacheNumberCoverageTest
{
    [Test]
    public void Cache_CoversAllSingleOperationResults()
    {
        // Arrange - Universal knowledge base built from Hard (contains Easy compositions)
        var knowledge = KnowledgeRegistry.Universal;

        // Act & Assert - should have all input numbers from Easy range
        for (int i = 1; i <= 10; i++)
        {
            Assert.DoesNotThrow(() => knowledge.Numbers.GetNumber(i), $"Missing input number {i}");
        }

        // Should have all addition results: 1+1=2 to 10+10=20
        for (int result = 2; result <= 20; result++)
        {
            Assert.DoesNotThrow(() => knowledge.Numbers.GetNumber(result), $"Missing addition result {result}");
        }

        // Should have common multiplication results
        Assert.DoesNotThrow(() => knowledge.Numbers.GetNumber(100), "Missing 10*10=100");
        Assert.DoesNotThrow(() => knowledge.Numbers.GetNumber(90), "Missing 9*10=90");
    }

    [Test]
    public void Cache_HandlesNegativeNumbersFromSubtraction()
    {
        // Arrange - Universal knowledge base contains negative numbers from Hard (superset of Medium)
        var knowledge = KnowledgeRegistry.Universal;

        // Act & Assert - should have negative results from subtraction
        // Hard has range -20..30, which encompasses Medium's -10..20
        Assert.DoesNotThrow(() => knowledge.Numbers.GetNumber(0), "Missing result 0");
        Assert.DoesNotThrow(() => knowledge.Numbers.GetNumber(-20), "Missing negative result");
        Assert.DoesNotThrow(() => knowledge.Numbers.GetNumber(-10), "Missing negative operand");
    }

    [Test]
    public void GetNumber_ThrowsForUncachedValue()
    {
        // Arrange - Universal knowledge base has wide range but not infinite
        var knowledge = KnowledgeRegistry.Universal;

        // Act & Assert - value way outside Hard ranges should not exist
        Assert.Throws<KeyNotFoundException>(() => knowledge.Numbers.GetNumber(1_000_000));
    }
}

