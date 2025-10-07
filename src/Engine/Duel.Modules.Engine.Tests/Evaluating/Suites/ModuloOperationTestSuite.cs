namespace Duel.Modules.Engine.Tests.Evaluating.Suites;

public sealed class ModuloOperationTestSuite : ExpressionEvaluatorTestSuite
{
    [Test]
    public void Modulo_BasicOperation_ReturnsRemainder()
    {
        AssertEvaluation("10 % 3", 1);
    }

    [Test]
    public void Modulo_EvenlyDivisible_ReturnsZero()
    {
        AssertEvaluation("15 % 5", 0);
    }

    [Test]
    public void Modulo_WithAddition_RespectsOrderOfOperations()
    {
        AssertEvaluation("10 + 7 % 3", 11); // 10 + (7 % 3) = 10 + 1
    }

    [Test]
    public void Modulo_WithMultiplication_RespectsOrderOfOperations()
    {
        AssertEvaluation("2 * 7 % 3", 2); // (2 * 7) % 3 = 14 % 3 = 2
    }

    [Test]
    public void Modulo_WithParentheses_RespectsGrouping()
    {
        AssertEvaluation("(10 + 7) % 3", 2); // 17 % 3 = 2
    }

    [Test]
    public void Modulo_LargerNumbers_ReturnsCorrectRemainder()
    {
        AssertEvaluation("100 % 17", 15);
    }
}

