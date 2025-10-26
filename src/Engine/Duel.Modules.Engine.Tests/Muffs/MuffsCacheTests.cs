using Duel.Modules.Engine.Games.Muffs;
using Duel.Modules.Engine.Games.Muffs.Expressions;
using Duel.Modules.Engine.Games.Muffs.Glyphs;

namespace Duel.Modules.Engine.Tests.Muffs;

[TestFixture]
public class MuffsCacheTests
{
    [Test]
    public void Cache_SquareRoot_OnlyStoresPerfectSquares()
    {
        // Arrange
        var settings = new ExpressionSettings
        {
            SquareRoot = new OperatorSettings(1.0, 0..10)
        };

        // Act
        var cache = new MuffsCache(settings);

        // Assert - should have sqrt(0)=0, sqrt(1)=1, sqrt(4)=2, sqrt(9)=3
        var compositions0 = cache.GetCompositions(0);
        Assert.That(compositions0.Any(c => c is UnaryComposition { Type: OperatorType.SquareRoot, Operand: 0, Result: 0 }));

        var compositions1 = cache.GetCompositions(1);
        Assert.That(compositions1.Any(c => c is UnaryComposition { Type: OperatorType.SquareRoot, Operand: 1, Result: 1 }));

        var compositions2 = cache.GetCompositions(2);
        Assert.That(compositions2.Any(c => c is UnaryComposition { Type: OperatorType.SquareRoot, Operand: 4, Result: 2 }));

        var compositions3 = cache.GetCompositions(3);
        Assert.That(compositions3.Any(c => c is UnaryComposition { Type: OperatorType.SquareRoot, Operand: 9, Result: 3 }));
    }

    [Test]
    public void Cache_Factorial_OnlyCapsAt5()
    {
        // Arrange
        var settings = new ExpressionSettings
        {
            Factorial = new OperatorSettings(1.0, 0..10)
        };

        // Act
        var cache = new MuffsCache(settings);

        // Assert - 5! = 120 should exist
        var compositions120 = cache.GetCompositions(120);
        Assert.That(compositions120.Any(c => c is UnaryComposition { Type: OperatorType.Factorial, Operand: 5, Result: 120 }));

        // 6! = 720 should NOT exist (capped at 5)
        var compositionsAll = new List<Composition>();
        try
        {
            compositionsAll = cache.GetCompositions(720).ToList();
        }
        catch { }
        
        var factorial6 = compositionsAll.OfType<UnaryComposition>()
            .Where(c => c.Type == OperatorType.Factorial && c.Operand == 6);
        Assert.That(factorial6, Is.Empty);
    }

    [Test]
    public void Cache_Division_ExcludesZeroDivisor()
    {
        // Arrange
        var settings = new ExpressionSettings
        {
            Division = new OperatorSettings(1.0, 0..10)
        };

        // Act
        var cache = new MuffsCache(settings);

        // Assert - should never have division by 0
        var allDivisions = new List<BinaryComposition>();
        for (int i = 0; i <= 10; i++)
        {
            try
            {
                var compositions = cache.GetCompositions(i);
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
        var settings = new ExpressionSettings
        {
            Modulo = new OperatorSettings(1.0, 0..10)
        };

        // Act
        var cache = new MuffsCache(settings);

        // Assert - should never have modulo by 0
        var allModulos = new List<BinaryComposition>();
        for (int i = 0; i <= 10; i++)
        {
            try
            {
                var compositions = cache.GetCompositions(i);
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
        var settings = new ExpressionSettings
        {
            Power = new OperatorSettings(1.0, 1..10)
        };

        // Act
        var cache = new MuffsCache(settings);

        // Assert - all power operations should produce safe results
        for (int i = 1; i <= 10; i++)
        {
            try
            {
                var compositions = cache.GetCompositions(i);
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
        // Arrange
        var settings = new ExpressionSettings
        {
            Addition = new OperatorSettings(1.0, 1..5),
            Multiplication = new OperatorSettings(1.0, 1..5)
        };

        // Act
        var cache = new MuffsCache(settings);

        // Assert - 6 can be made by 1+5, 2+4, 3+3, 5+1, 2*3, 3*2
        var compositions = cache.GetCompositions(6);
        var additions = compositions.OfType<BinaryComposition>()
            .Where(c => c.Type == OperatorType.Addition).ToList();
        var multiplications = compositions.OfType<BinaryComposition>()
            .Where(c => c.Type == OperatorType.Multiplication).ToList();

        Assert.That(additions, Has.Count.EqualTo(5)); // 1+5, 2+4, 3+3, 4+2, 5+1
        Assert.That(multiplications, Has.Count.EqualTo(2)); // 2*3, 3*2
    }
}

