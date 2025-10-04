namespace Duel.Core.Tests.Muffs.Evaluating.Suites;

[TestFixture]
public sealed class ConstantsTestSuite : ExpressionEvaluatorTestSuite
{
    [Test]
    public void Evaluate_Constant_Zero()
    {
        AssertEvaluation("0", expected: 0);
    }

    [Test]
    public void Evaluate_Constant_Positive()
    {
        AssertEvaluation("42", expected: 42);
    }

    [Test]
    public void Evaluate_Constant_Negative_ThrowsFormatException()
    {
        Assert.Throws<FormatException>(() => GetEvaluation("-5"), "Negative literals are currently unsupported.");
    }
}