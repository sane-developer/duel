using Duel.Core.Games.Muffs;

namespace Duel.Core.Tests.Games.Muffs;

public class ExpressionGeneratorTests
{
    private Random _rng;
    
    private ExpressionGenerator _generator;
    
    private ExpressionPolicy _policy;
    
    [SetUp]
    public void Setup()
    {
        _rng = new Random(42);
        _generator = new ExpressionGenerator(_rng);
        _policy = new ExpressionPolicy(
            2, 
            200, 
            25, 
            40, 
            10, 
            3, 
            5
        );
    }
    
    [Test]
    public void Generator_Produces_Valid_Expressions()
    {
        var expression = _generator.Generate(_policy);

        var stringified = ExpressionSerializer.ToFullyParenthesized(expression);
        
        var result = ExpressionEvaluator.Evaluate(expression);
        
        Assert.That(result, Is.LessThanOrEqualTo(_policy.MaxAbsoluteValue));
    }
}