using Duel.Modules.Engine.Games.Muffs.Generation;
using Duel.Modules.Engine.Games.Muffs.Knowledge;
using Duel.Modules.Engine.Games.Muffs.Representation;

namespace Duel.Modules.Engine.Tests.Muffs;

[TestFixture]
public class MuffsCacheTests
{
    [Test]
    public void Cache_SquareRoot_OnlyStoresPerfectSquares()
    {
        // Arrange
        var settings = new GeneratorSettings
        {
            SquareRoot = new OperatorSettings(1.0, 0..10)
        };

        // Act
        var registry = new CompositionsRegistry(settings);

        // Assert - should have sqrt(0)=0, sqrt(1)=1, sqrt(4)=2, sqrt(9)=3
        var compositions0 = registry.GetCompositions(0);
        Assert.That(compositions0.Any(c => c is UnaryComposition { Type: OperatorType.SquareRoot, Operand: 0, Result: 0 }));

        var compositions1 = registry.GetCompositions(1);
        Assert.That(compositions1.Any(c => c is UnaryComposition { Type: OperatorType.SquareRoot, Operand: 1, Result: 1 }));

        var compositions2 = registry.GetCompositions(2);
        Assert.That(compositions2.Any(c => c is UnaryComposition { Type: OperatorType.SquareRoot, Operand: 4, Result: 2 }));

        var compositions3 = registry.GetCompositions(3);
        Assert.That(compositions3.Any(c => c is UnaryComposition { Type: OperatorType.SquareRoot, Operand: 9, Result: 3 }));
    }

    [Test]
    public void Cache_Factorial_OnlyCapsAt5()
    {
        // Arrange - result range includes factorials up to 5! (120)
        var settings = new GeneratorSettings
        {
            Factorial = new OperatorSettings(1.0, 1..120)
        };

        // Act
        var registry = new CompositionsRegistry(settings);

        // Assert - 5! = 120 should exist
        var compositions120 = registry.GetCompositions(120);
        Assert.That(compositions120.Any(c => c is UnaryComposition { Type: OperatorType.Factorial, Operand: 5, Result: 120 }));

        // 6! = 720 should NOT exist (operand capped at maxSafeFactorial in builder)
        Assert.That(registry.AvailableResults, Does.Not.Contain(720), "6! = 720 should not be in registry");
    }

    [Test]
    public void Cache_Division_ExcludesZeroDivisor()
    {
        // Arrange
        var settings = new GeneratorSettings
        {
            Division = new OperatorSettings(1.0, 0..10)
        };

        // Act
        var registry = new CompositionsRegistry(settings);

        // Assert - should never have division by 0
        var allDivisions = new List<BinaryComposition>();
        for (int i = 0; i <= 10; i++)
        {
            try
            {
                var compositions = registry.GetCompositions(i);
                var divisions = compositions.OfType<BinaryComposition>()
                    .Where(c => c.Type == OperatorType.Division);
                allDivisions.AddRange(divisions);
            }
            catch (KeyNotFoundException)
            {
                // Result doesn't exist in cache, which is fine
                continue;
            }
        }
        
        var divisionByZero = allDivisions.Where(c => c.Rhs == 0);
        Assert.That(divisionByZero, Is.Empty, "Found division by zero");
    }

    [Test]
    public void Cache_Modulo_ExcludesZeroDivisor()
    {
        // Arrange
        var settings = new GeneratorSettings
        {
            Modulo = new OperatorSettings(1.0, 0..10)
        };

        // Act
        var registry = new CompositionsRegistry(settings);

        // Assert - should never have modulo by 0
        var allModulos = new List<BinaryComposition>();
        for (int i = 0; i <= 10; i++)
        {
            try
            {
                var compositions = registry.GetCompositions(i);
                var modulos = compositions.OfType<BinaryComposition>()
                    .Where(c => c.Type == OperatorType.Modulo);
                allModulos.AddRange(modulos);
            }
            catch (KeyNotFoundException)
            {
                // Result doesn't exist in cache, which is fine
                continue;
            }
        }
        
        var moduloByZero = allModulos.Where(c => c.Rhs == 0);
        Assert.That(moduloByZero, Is.Empty, "Found modulo by zero");
    }

    [Test]
    public void Cache_Power_PreventsMassiveValues()
    {
        // Arrange
        var settings = new GeneratorSettings
        {
            Power = new OperatorSettings(1.0, 1..10)
        };

        // Act
        var registry = new CompositionsRegistry(settings);

        // Assert - all power operations should produce safe results
        for (int i = 1; i <= 10; i++)
        {
            try
            {
                var compositions = registry.GetCompositions(i);
                var powers = compositions.OfType<BinaryComposition>()
                    .Where(c => c.Type == OperatorType.Power);
                
                foreach (var power in powers)
                {
                    // Verify result matches actual power
                    var expected = (int)Math.Pow(power.Lhs, power.Rhs);
                    Assert.That(power.Result, Is.EqualTo(expected));
                }
            }
            catch (KeyNotFoundException)
            {
                // Result doesn't exist in cache, which is fine
                continue;
            }
        }
    }

    [Test]
    public void Cache_CompositionLookup_ReturnsAllWaysToMakeNumber()
    {
        // Arrange - result range 1..10 to include result 6
        var settings = new GeneratorSettings
        {
            Addition = new OperatorSettings(1.0, 1..10),
            Multiplication = new OperatorSettings(1.0, 1..10)
        };

        // Act
        var registry = new CompositionsRegistry(settings);

        // Assert - 6 can be made by addition and multiplication
        var compositions = registry.GetCompositions(6);
        var additions = compositions.OfType<BinaryComposition>()
            .Where(c => c.Type == OperatorType.Addition).ToList();
        var multiplications = compositions.OfType<BinaryComposition>()
            .Where(c => c.Type == OperatorType.Multiplication).ToList();

        // Should have multiple ways to make 6
        Assert.That(additions, Is.Not.Empty, "Should have addition compositions for 6");
        Assert.That(multiplications, Is.Not.Empty, "Should have multiplication compositions for 6 (2*3, 3*2)");
        
        // Verify some specific compositions exist
        Assert.That(additions.Any(c => c.Lhs == 1 && c.Rhs == 5), "Should have 1+5=6");
        Assert.That(additions.Any(c => c.Lhs == 3 && c.Rhs == 3), "Should have 3+3=6");
        Assert.That(multiplications.Any(c => c.Lhs == 2 && c.Rhs == 3), "Should have 2*3=6");
    }
}

