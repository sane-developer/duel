using BenchmarkDotNet.Attributes;
using Duel.Modules.Engine.Muffs.Expressions;
using Duel.Modules.Engine.Muffs.Expressions.Presets;
using Duel.Modules.Engine.Muffs.Expressions.Presets.Modes;
using Duel.Modules.Engine.Muffs.Glyphs;

namespace Duel.Modules.Engine.Diagnostics.Muffs;

[MemoryDiagnoser]
[SimpleJob(warmupCount: 3, iterationCount: 10)]
public class ExpressionGenerationBenchmarks
{
    private readonly Random _rng = new(42);

    private ExpressionGenerator _easyGenerator = null!;

    private ExpressionGenerator _mediumGenerator = null!;

    private ExpressionGenerator _hardGenerator = null!;

    [GlobalSetup]
    public void Setup()
    {
        _easyGenerator = new ExpressionGenerator(new ExpressionGeneratorContext(_rng, new EasyExpressionPreset()));
        
        _mediumGenerator = new ExpressionGenerator(new ExpressionGeneratorContext(_rng, new MediumExpressionPreset()));
        
        _hardGenerator = new ExpressionGenerator(new ExpressionGeneratorContext(_rng, new HardExpressionPreset()));
    }
    
    [Benchmark(Description = "Easy Mode Generation")]
    public void GenerateEasyMode()
    {
        for (var i = 0; i < 1000; i++)
        {
            _easyGenerator.Generate();
        }
    }
    
    [Benchmark(Description = "Medium Mode Generation")]
    public void GenerateMediumMode()
    {
        for (var i = 0; i < 1000; i++)
        {
            _mediumGenerator.Generate();
        }
    }
    
    [Benchmark(Description = "Hard Mode Generation", Baseline = true)]
    public void GenerateHardMode()
    {
        for (var i = 0; i < 1000; i++)
        {
            _hardGenerator.Generate();
        }
    }
    
    [Benchmark(Description = "Single Easy Expression")]
    public Glyph GenerateSingleEasy()
    {
        return _easyGenerator.Generate();
    }
    
    [Benchmark(Description = "Single Medium Expression")]
    public Glyph GenerateSingleMedium()
    {
        return _mediumGenerator.Generate();
    }
    
    [Benchmark(Description = "Single Hard Expression")]
    public Glyph GenerateSingleHard()
    {
        return _hardGenerator.Generate();
    }
}

