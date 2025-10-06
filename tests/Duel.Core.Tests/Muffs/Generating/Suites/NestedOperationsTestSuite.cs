using Duel.Core.Games.Muffs.AST;
using Duel.Core.Games.Muffs.Evaluating;
using Duel.Core.Games.Muffs.Generating;

namespace Duel.Core.Tests.Muffs.Generating.Suites;

[TestFixture]
public sealed class NestedOperationsTestSuite : ExpressionGeneratorTestSuite
{
    private Random _rng;
    
    [SetUp]
    public void Setup()
    {
        _rng = new Random(42);
    }

    [Test]
    public void Generate_NestedExpression_RespectsDepthLimit()
    {
        var settings = CreateNestedSettings(maxDepth: 3);
        
        var expression = GetExpression(settings, _rng);
        
        var depth = CalculateDepth(expression);

        Assert.That(depth, Is.LessThanOrEqualTo(3), "Expression depth must respect settings limit");
    }

    [Test]
    public void Generate_NestedDivision_ProducesExactDivisor()
    {
        var settings = CreateDivisionOnlySettings(maxDepth: 3);
        
        var expression = GetExpression(settings, _rng);
        
        AssertAllDivisionsExact(expression);
    }

    [Test]
    public void Generate_DeepNesting_EvaluatesWithoutError()
    {
        var settings = CreateNestedSettings(maxDepth: 4);
        
        var expression = GetExpression(settings, _rng);
        
        var evaluator = new ExpressionEvaluator(expression);

        Assert.DoesNotThrow(() => evaluator.Evaluate(), "Deep nested expression should evaluate");
    }

    [Test]
    public void Generate_MixedOperators_CreatesVariedStructure()
    {
        var settings = CreateNestedSettings(maxDepth: 3);
        
        var expression = GetExpression(settings, _rng);
        
        Assert.That(expression, Is.InstanceOf<Binary>(), "Should generate nested operations, not just constant");
    }

    [Test]
    public void Generate_MultipleExpressions_ProduceDifferentResults()
    {
        var settings = CreateNestedSettings(maxDepth: 3);
        
        var expr1 = GetExpression(settings, new Random(1));
        
        var expr2 = GetExpression(settings, new Random(2));
        
        var eval1 = new ExpressionEvaluator(expr1).Evaluate();
        
        var eval2 = new ExpressionEvaluator(expr2).Evaluate();
        
        Assert.That(eval1, Is.Not.EqualTo(eval2), "Different seeds should produce different expressions");
    }

    private static ExpressionSettings CreateNestedSettings(int maxDepth)
    {
        return new ExpressionSettings(
            Depth: ConstantSettings.From(minimum: 2, maximum: maxDepth),
            Constant: ConstantSettings.From(minimum: 1, maximum: 10),
            Exponent: ConstantSettings.From(minimum: 2, maximum: 3),
            Add: OperatorSettings.From(enabled: true, weight: 1.0d),
            Subtract: OperatorSettings.From(enabled: true, weight: 1.0d),
            Multiply: OperatorSettings.From(enabled: true, weight: 0.8d),
            Divide: OperatorSettings.From(enabled: true, weight: 0.5d),
            Power: OperatorSettings.From(enabled: true, weight: 0.3d)
        );
    }

    private static ExpressionSettings CreateDivisionOnlySettings(int maxDepth)
    {
        return new ExpressionSettings(
            Depth: ConstantSettings.From(minimum: 2, maximum: maxDepth),
            Constant: ConstantSettings.From(minimum: 1, maximum: 10),
            Exponent: ConstantSettings.From(minimum: 2, maximum: 3),
            Add: OperatorSettings.From(enabled: false, weight: 0d),
            Subtract: OperatorSettings.From(enabled: false, weight: 0d),
            Multiply: OperatorSettings.From(enabled: false, weight: 0d),
            Divide: OperatorSettings.From(enabled: true, weight: 1.0d),
            Power: OperatorSettings.From(enabled: false, weight: 0d)
        );
    }

    private static int CalculateDepth(Expression expression)
    {
        return expression switch
        {
            Constant => 0,
            Binary binary => 1 + Math.Max(CalculateDepth(binary.Left), CalculateDepth(binary.Right)),
            _ => 0
        };
    }

    private static void AssertAllDivisionsExact(Expression expression)
    {
        if (expression is Division division)
        {
            var dividend = new ExpressionEvaluator(division.Left).Evaluate();
            
            var divisor = new ExpressionEvaluator(division.Right).Evaluate();
            
            IsValidDivisor(dividend, divisor);
            
            AssertAllDivisionsExact(division.Left);
            
            AssertAllDivisionsExact(division.Right);

            return;
        }

        if (expression is Binary binary)
        {
            AssertAllDivisionsExact(binary.Left);

            AssertAllDivisionsExact(binary.Right);
        }
    }
}