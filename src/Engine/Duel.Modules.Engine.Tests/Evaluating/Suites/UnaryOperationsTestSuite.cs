using Duel.Modules.Engine.Games.Muffs.Evaluating;

namespace Duel.Modules.Engine.Tests.Evaluating.Suites;

public sealed class UnaryOperationsTestSuite : ExpressionEvaluatorTestSuite
{
    [Test]
    public void Negation_PositiveNumber_ReturnsNegative()
    {
        var expression = "minus(5)";

        AssertEvaluation(expression, -5);
    }

    [Test]
    public void Negation_NegativeNumber_ReturnsPositive()
    {
        var expression = "minus(minus(5))";

        var eval = new ExpressionEvaluator(GetExpression(expression));
        var result = eval.Evaluate();

        Assert.That(result, Is.EqualTo(5));
    }

    [Test]
    public void Abs_PositiveNumber_ReturnsSame()
    {
        var expression = "abs(5)";

        var eval = new ExpressionEvaluator(GetExpression(expression));
        var result = eval.Evaluate();

        Assert.That(result, Is.EqualTo(5));
    }

    [Test]
    public void Abs_NegativeNumber_ReturnsPositive()
    {
        var expression = "abs(minus(5))";

        var eval = new ExpressionEvaluator(GetExpression(expression));
        var result = eval.Evaluate();

        Assert.That(result, Is.EqualTo(5));
    }

    [Test]
    public void Sqrt_PerfectSquare_ReturnsRoot()
    {
        var expression = "sqrt(16)";

        var eval = new ExpressionEvaluator(GetExpression(expression));
        var result = eval.Evaluate();

        Assert.That(result, Is.EqualTo(4));
    }

    [Test]
    public void Sqrt_LargerPerfectSquare_ReturnsRoot()
    {
        var expression = "sqrt(144)";

        var eval = new ExpressionEvaluator(GetExpression(expression));
        var result = eval.Evaluate();

        Assert.That(result, Is.EqualTo(12));
    }

    [Test]
    public void Factorial_Zero_ReturnsOne()
    {
        var expression = "factorial(0)";

        var eval = new ExpressionEvaluator(GetExpression(expression));
        var result = eval.Evaluate();

        Assert.That(result, Is.EqualTo(1));
    }

    [Test]
    public void Factorial_One_ReturnsOne()
    {
        var expression = "factorial(1)";

        var eval = new ExpressionEvaluator(GetExpression(expression));
        var result = eval.Evaluate();

        Assert.That(result, Is.EqualTo(1));
    }

    [Test]
    public void Factorial_Five_Returns120()
    {
        var expression = "factorial(5)";

        var eval = new ExpressionEvaluator(GetExpression(expression));
        var result = eval.Evaluate();

        Assert.That(result, Is.EqualTo(120));
    }

    [Test]
    public void UnaryWithBinary_AbsOfSubtraction_ReturnsCorrectResult()
    {
        var expression = "abs(5 - 10)";

        var eval = new ExpressionEvaluator(GetExpression(expression));
        var result = eval.Evaluate();

        Assert.That(result, Is.EqualTo(5));
    }

    [Test]
    public void UnaryWithBinary_SqrtOfAddition_ReturnsCorrectResult()
    {
        var expression = "sqrt(9 + 16)";

        var eval = new ExpressionEvaluator(GetExpression(expression));
        var result = eval.Evaluate();

        Assert.That(result, Is.EqualTo(5));
    }

    [Test]
    public void UnaryWithBinary_FactorialOfSum_ReturnsCorrectResult()
    {
        var expression = "factorial(2 + 3)";

        var eval = new ExpressionEvaluator(GetExpression(expression));
        var result = eval.Evaluate();

        Assert.That(result, Is.EqualTo(120)); // 5! = 120
    }

    [Test]
    public void NestedUnary_AbsOfNegation_ReturnsPositive()
    {
        var expression = "abs(minus(7))";

        var eval = new ExpressionEvaluator(GetExpression(expression));
        var result = eval.Evaluate();

        Assert.That(result, Is.EqualTo(7));
    }

    [Test]
    public void NestedUnary_SqrtOfAbsOfNegation_ReturnsCorrectResult()
    {
        var expression = "sqrt(abs(minus(16)))";

        var eval = new ExpressionEvaluator(GetExpression(expression));
        var result = eval.Evaluate();

        Assert.That(result, Is.EqualTo(4));
    }

    [Test]
    public void ComplexExpression_DivisionWithUnary_ReturnsCorrectResult()
    {
        var expression = "(2 + 5) / abs(minus(7))";

        var eval = new ExpressionEvaluator(GetExpression(expression));
        var result = eval.Evaluate();

        Assert.That(result, Is.EqualTo(1)); // 7 / 7 = 1
    }

    [Test]
    public void ComplexExpression_MultipleUnary_ReturnsCorrectResult()
    {
        var expression = "abs(minus(3)) * sqrt(16) + factorial(3)";

        var eval = new ExpressionEvaluator(GetExpression(expression));
        var result = eval.Evaluate();

        Assert.That(result, Is.EqualTo(18)); // 3 * 4 + 6 = 18
    }
}

