using Duel.Modules.Engine.Muffs.Compositions;
using Duel.Modules.Engine.Muffs.Expressions;
using Duel.Modules.Engine.Muffs.Expressions.Presets;
using Duel.Modules.Engine.Muffs.Expressions.Presets.Modes;
using Duel.Modules.Engine.Muffs.Glyphs;

namespace Duel.Modules.Engine.Tests.Muffs.Regression;

[TestFixture]
public sealed class CompositionRegressionTests
{
    private readonly Random _rng = new(42);

    private static readonly IExpressionPreset[] _presets =
    [
        new EasyExpressionPreset(),
        new MediumExpressionPreset(),
        new HardExpressionPreset()
    ];

    /// <summary>
    /// Regression test for division by zero bug discovered during benchmarking on 2025-11-20.
    /// 
    /// Issue: Division compositions could contain zero operands (e.g., (5 - 5) / 3),
    /// causing DivideByZeroException during evaluation.
    /// 
    /// Fix: Added NonZeroDivisorFilter to reject division compositions where either operand is zero.
    /// </summary>
    [TestCaseSource(nameof(_presets))]
    public void GeneratedExpressions_ShouldNeverCauseDivisionByZero(IExpressionPreset preset)
    {
        //
        //  Arrange
        //

        var context = new ExpressionGeneratorContext(_rng, preset);

        var generator = new ExpressionGenerator(context);

        const int testIterations = 10_000;

        //
        //  Act & Assert
        //

        for (var i = 0; i < testIterations; i++)
        {
            var expression = generator.Generate();

            Assert.DoesNotThrow(() => AssertResultIsInteger(expression), $"Failed on iteration {i} for {preset.GetType().Name}");

            static void AssertResultIsInteger(Glyph expression)
            {
                var result = ExpressionEvaluator.Evaluate(expression);
                    
                Assert.That(result, Is.TypeOf<int>());
            }
        }
    }

    /// <summary>
    ///     Verifies that division and modulo compositions in the registry never have zero operands.
    /// </summary>
    /// <remarks>
    ///     Easy mode doesn't have divisions, so we only test Medium and Hard.
    /// </remarks>
    [TestCase(typeof(MediumExpressionPreset))]
    [TestCase(typeof(HardExpressionPreset))]
    public void CompositionRegistry_DivisionCompositions_ShouldNeverHaveZeroOperands(Type presetType)
    {
        //
        //  Arrange
        //

        var preset = (IExpressionPreset) Activator.CreateInstance(presetType)!;

        var registry = preset.CompositionRegistry;

        //
        //  Act
        //

        var allDivisionCompositions = Enumerable
            .Range(start: -100, count: 201)
            .SelectMany(result => registry.GetCompositions(result, GlyphType.Divide)
                                          .Concat(registry.GetCompositions(result, GlyphType.Modulo)))
            .OfType<BinaryComposition>()
            .ToList();

        //
        //  Assert
        //

        Assert.That(allDivisionCompositions, Is.Not.Empty, $"No division/modulo compositions found in {presetType.Name}");

        foreach (var composition in allDivisionCompositions)
        {
            Assert.Multiple(() =>
            {
                Assert.That(composition.Lhs, Is.Not.EqualTo(0), $"Division/Modulo composition has zero left operand: {composition.Lhs} {(composition.OperatorType == GlyphType.Divide ? "/" : "%")} {composition.Rhs} = {composition.Result}");

                Assert.That(composition.Rhs, Is.Not.EqualTo(0), $"Division/Modulo composition has zero right operand: {composition.Lhs} {(composition.OperatorType == GlyphType.Divide ? "/" : "%")} {composition.Rhs} = {composition.Result}");
            });
        }
    }
}

