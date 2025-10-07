using Duel.Modules.Engine.Games.Muffs.AST;
using Duel.Modules.Engine.Games.Muffs.Evaluating;

namespace Duel.Modules.Engine.Tests.Generating.Suites;

[TestFixture]
public sealed class SingleOperationsTestSuite : ExpressionGeneratorTestSuite
{
    private Random _rng;
    
    [SetUp]
    public void Setup()
    {
        _rng = new Random(42);
    }

    [Test]
    public void Generate_Constant_WithinRange()
    {
        var settings = CreateConstantOnlySettings();
        
        var expression = GetExpression(settings, _rng);

        IsValidConstant(expression);
    }

    [Test]
    public void Generate_Addition_ValidStructure()
    {
        var settings = CreateSingleOperatorSettings(Operator.Code.Add);
        
        var expression = GetExpression(settings, _rng);

        IsValidBinary<Addition>(expression);
    }

    [Test]
    public void Generate_Subtraction_ValidStructure()
    {
        var settings = CreateSingleOperatorSettings(Operator.Code.Subtract);
       
        var expression = GetExpression(settings, _rng);

        IsValidBinary<Subtraction>(expression);
    }

    [Test]
    public void Generate_Multiplication_ValidStructure()
    {
        var settings = CreateSingleOperatorSettings(Operator.Code.Multiply);
        
        var expression = GetExpression(settings, _rng);

        IsValidBinary<Multiplication>(expression);
    }

    [Test]
    public void Generate_Division_ExactDivisor()
    {
        var settings = CreateSingleOperatorSettings(Operator.Code.Divide);
        
        var expression = GetExpression(settings, _rng);

        var division = IsValidBinary<Division>(expression);

        var dividend = division.Left.As<Constant>().Value;

        var divisor = division.Right.As<Constant>().Value;
       
        IsValidDivisor(dividend, divisor);
    }

    [Test]
    public void Generate_Power_ExponentInRange()
    {
        var settings = CreateSingleOperatorSettings(Operator.Code.Power);
        
        var expression = GetExpression(settings, _rng).As<Binary>();

        var power = IsValidBinary<Power>(expression);

        var exponent = power.Right.As<Constant>().Value;

        IsValidExponent(exponent);
    }

    [Test]
    public void Generate_SingleOperation_EvaluatesWithoutError()
    {
        var settings = CreateSingleOperatorSettings(Operator.Code.Add);
        
        var expression = GetExpression(settings, _rng);

        var evaluator = new ExpressionEvaluator(expression);
        
        Assert.DoesNotThrow(() => evaluator.Evaluate(), "Generated expression should evaluate without errors");
    }
}