using Duel.Modules.Engine.Muffs.Expressions;
using Duel.Modules.Engine.Muffs.Expressions.Presets;
using Duel.Modules.Engine.Muffs.Expressions.Presets.Modes;
using Duel.Modules.Engine.Muffs.Glyphs;
using System.Diagnostics;

namespace Duel.Modules.Engine.Tests.Muffs.Latency;

[TestFixture]
public sealed class ExpressionSerializationLatencyTests
{
    private readonly Random _rng = new(42);

    private static readonly IExpressionPreset[] _presets =
    [
        new EasyExpressionPreset(),
        new MediumExpressionPreset(),
        new HardExpressionPreset()
    ];

    [TestCaseSource(nameof(_presets))]
    public void Serialize_MeetsLatencyTarget(IExpressionPreset preset)
    {
        //
        //  Arrange
        //

        var context = new ExpressionGeneratorContext(_rng, preset);
        
        var generator = new ExpressionGenerator(context);
        
        var expressions = new List<Glyph>();
        
        for (var i = 0; i < 1000; i++)
        {
            var expression = generator.Generate();
            
            expressions.Add(expression);
        }
        
        //
        //  Act
        //

        var sw = Stopwatch.StartNew();
        
        foreach (var expression in expressions)
        {
            ExpressionSerializer.Serialize(expression);
        }
        
        sw.Stop();
        
        //
        //  Assert
        //

        var avgMs = sw.ElapsedMilliseconds / 1000.0d;
        
        Assert.That(avgMs, Is.LessThan(2.0), $"Serialization should be < 2ms but was {avgMs:F3}ms");
    }
    
    [Test]
    public void Serialize_ProducesReasonableSize()
    {        
        //
        //  Act & Assert
        //

        foreach (var preset in _presets)
        {
            var context = new ExpressionGeneratorContext(_rng, preset);
            
            var generator = new ExpressionGenerator(context);
            
            var sizes = new List<int>();
            
            for (var i = 0; i < 100; i++)
            {
                var expression = generator.Generate();
                
                var serialized = ExpressionSerializer.Serialize(expression);
                
                sizes.Add(serialized.Length);
            }
            
            var avgSize = sizes.Average();
            
            var maxSize = sizes.Max();
            
            var maxExpected = preset switch
            {
                EasyExpressionPreset => 100,
                MediumExpressionPreset => 250,
                HardExpressionPreset => 700,
                _ => 500
            };
            
            Assert.That(maxSize, Is.LessThan(maxExpected), $"{preset.GetType().Name} max size should be reasonable (< {maxExpected} bytes)");
        }
    }
    
    [Test]
    public void Serialize_ScalesLinearlyWithComplexity()
    {
        //
        //  Arrange
        //

        var hardPreset = _presets.Last();

        var context = new ExpressionGeneratorContext(_rng, hardPreset);

        var generator = new ExpressionGenerator(context);
        
        var results = new Dictionary<string, double>();
        
        //
        //  Act
        //

        foreach (var count in new[] { 100, 500, 1000, 2000 })
        {
            var expressions = new List<Glyph>();

            for (var i = 0; i < count; i++)
            {
                var expression = generator.Generate();

                expressions.Add(expression);
            }
            
            var sw = Stopwatch.StartNew();
            
            foreach (var expression in expressions)
            {
                ExpressionSerializer.Serialize(expression);
            }
            
            sw.Stop();
            
            var avgMs = sw.ElapsedMilliseconds / (double) count;

            var key = count.ToString();

            results[key] = avgMs;
        }
        
        //
        //  Assert
        //

        var timings = results.Values.ToList();
        
        var minTiming = timings.Min();
        
        var maxTiming = timings.Max();
        
        if (maxTiming < 0.01)
        {
            Assert.Pass("Serialization is too fast to measure variance accurately");
        }
        
        var variance = maxTiming / minTiming;
        
        Assert.That(variance, Is.LessThan(3.0), "Serialization time should be roughly constant (linear scaling)");
    }
}
