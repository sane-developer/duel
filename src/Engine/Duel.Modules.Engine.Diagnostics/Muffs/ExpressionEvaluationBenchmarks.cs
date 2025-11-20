using BenchmarkDotNet.Attributes;
using Duel.Modules.Engine.Muffs.Expressions;
using Duel.Modules.Engine.Muffs.Expressions.Presets;
using Duel.Modules.Engine.Muffs.Expressions.Presets.Modes;
using Duel.Modules.Engine.Muffs.Glyphs;

namespace Duel.Modules.Engine.Diagnostics.Muffs;

[MemoryDiagnoser]
[SimpleJob(warmupCount: 3, iterationCount: 10)]
public class ExpressionEvaluationBenchmarks
{
    private readonly Random _rng = new(42);
    
    private List<Glyph> _easyExpressions = null!;
    
    private List<Glyph> _mediumExpressions = null!;
    
    private List<Glyph> _hardExpressions = null!;
    
    [GlobalSetup]
    public void Setup()
    {
        _easyExpressions = GenerateExpressions(new EasyExpressionPreset(), 1000);
        
        _mediumExpressions = GenerateExpressions(new MediumExpressionPreset(), 1000);
        
        _hardExpressions = GenerateExpressions(new HardExpressionPreset(), 1000);
    }
    
    [Benchmark(Description = "Easy Mode Evaluation")]
    public void EvaluateEasyMode()
    {
        foreach (var expression in _easyExpressions)
        {
            ExpressionEvaluator.Evaluate(expression);
        }
    }
    
    [Benchmark(Description = "Medium Mode Evaluation")]
    public void EvaluateMediumMode()
    {
        foreach (var expression in _mediumExpressions)
        {
            ExpressionEvaluator.Evaluate(expression);
        }
    }
    
    [Benchmark(Description = "Hard Mode Evaluation", Baseline = true)]
    public void EvaluateHardMode()
    {
        foreach (var expression in _hardExpressions)
        {
            ExpressionEvaluator.Evaluate(expression);
        }
    }
    
    [Benchmark(Description = "Single Easy Expression")]
    public int EvaluateSingleEasy()
    {
        return ExpressionEvaluator.Evaluate(_easyExpressions[0]);
    }
    
    [Benchmark(Description = "Single Medium Expression")]
    public int EvaluateSingleMedium()
    {
        return ExpressionEvaluator.Evaluate(_mediumExpressions[0]);
    }
    
    [Benchmark(Description = "Single Hard Expression")]
    public int EvaluateSingleHard()
    {
        return ExpressionEvaluator.Evaluate(_hardExpressions[0]);
    }
    
    private List<Glyph> GenerateExpressions(IExpressionPreset preset, int count)
    {
        var context = new ExpressionGeneratorContext(_rng, preset);
        
        var generator = new ExpressionGenerator(context);
        
        var expressions = new List<Glyph>(count);
        
        for (var i = 0; i < count; i++)
        {
            var expression = generator.Generate();

            expressions.Add(expression);
        }
        
        return expressions;
    }
}

