using BenchmarkDotNet.Attributes;
using Duel.Modules.Engine.Muffs.Expressions;
using Duel.Modules.Engine.Muffs.Expressions.Presets;
using Duel.Modules.Engine.Muffs.Expressions.Presets.Modes;
using Duel.Modules.Engine.Muffs.Glyphs;

namespace Duel.Modules.Engine.Diagnostics.Muffs;

[MemoryDiagnoser]
[SimpleJob(warmupCount: 3, iterationCount: 10)]
public class ExpressionSerializationBenchmarks
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

    [Benchmark(Description = "Easy Mode Serialization")]
    public void SerializeEasyMode()
    {
        foreach (var expression in _easyExpressions)
        {
            ExpressionSerializer.Serialize(expression);
        }
    }
    
    [Benchmark(Description = "Medium Mode Serialization")]
    public void SerializeMediumMode()
    {
        foreach (var expression in _mediumExpressions)
        {
            ExpressionSerializer.Serialize(expression);
        }
    }
    
    [Benchmark(Description = "Hard Mode Serialization", Baseline = true)]
    public void SerializeHardMode()
    {
        foreach (var expression in _hardExpressions)
        {
            ExpressionSerializer.Serialize(expression);
        }
    }
    
    [Benchmark(Description = "Single Easy Expression")]
    public string SerializeSingleEasy()
    {
        return ExpressionSerializer.Serialize(_easyExpressions[0]);
    }
    
    [Benchmark(Description = "Single Medium Expression")]
    public string SerializeSingleMedium()
    {
        return ExpressionSerializer.Serialize(_mediumExpressions[0]);
    }
    
    [Benchmark(Description = "Single Hard Expression")]
    public string SerializeSingleHard()
    {
        return ExpressionSerializer.Serialize(_hardExpressions[0]);
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

