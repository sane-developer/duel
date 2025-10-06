namespace Duel.Core.Tests.Muffs.Evaluating.Suites;

[TestFixture]
public sealed class SingleOperationsTestSuite : ExpressionEvaluatorTestSuite
{
    [Test]
    public void Evaluate_Constant_Positive()
    {
        AssertEvaluation("5", 5);
    }

    [Test]
    public void Evaluate_Constant_Negative_ShouldThrow()
    {
        Assert.Throws<FormatException>(() => AssertEvaluation("-5", -5), "Currently unsupported.");
    }

    [Test]
    public void Evaluate_Addition_Exact()
    {
        AssertEvaluation("100 + 200", expected: 300);
    }

    [Test]
    public void Evaluate_Subtraction_Exact()
    {
        AssertEvaluation("500 - 300", expected: 200);
    }

    [Test]
    public void Evaluate_Multiplication_Exact()
    {
        AssertEvaluation("5 * 3", expected: 15);
    }

    [Test]
    public void Evaluate_Division_Exact()
    {
        AssertEvaluation("100 / 25", expected: 4);
    }

    [Test]
    public void Evaluate_Power_Exact()
    {
        AssertEvaluation("3 ^ 4", expected: 81);
    }
}