namespace Duel.Core.Tests.Muffs.Evaluating.Suites;

[TestFixture]
public sealed class NestedOperationsTestSuite : ExpressionEvaluatorTestSuite
{
    [Test]
    public void Evaluate_AdditionChain_LeftAssociative()
    {
        AssertEvaluation("1 + 2 + 3 + 4", 10);
    }

    [Test]
    public void Evaluate_SubtractionChain_LeftAssociative()
    {
        AssertEvaluation("100 - 20 - 30 - 10", 40);
    }

    [Test]
    public void Evaluate_MultiplicationChain_LeftAssociative()
    {
        AssertEvaluation("2 * 3 * 4", 24);
    }

    [Test]
    public void Evaluate_DivisionChain_LeftAssociative()
    {
        AssertEvaluation("100 / 5 / 2", 10);
    }

    [Test]
    public void Evaluate_PowerChain_RightAssociative()
    {
        AssertEvaluation("2 ^ 3 ^ 2", 512);
    }

    [Test]
    public void Evaluate_MixedAddSubChain_LeftAssociative()
    {
        AssertEvaluation("50 - 10 + 5 - 15", 30);
    }

    [Test]
    public void Evaluate_MixedMulDivChain_LeftAssociative()
    {
        AssertEvaluation("120 / 6 * 2 / 4", 10);
    }

    [Test]
    public void Evaluate_Precedence_MultiplicationOverAddition()
    {
        AssertEvaluation("2 + 3 * 4", 14);
    }

    [Test]
    public void Evaluate_Precedence_DivisionOverSubtraction()
    {
        AssertEvaluation("100 - 60 / 3", 80);
    }

    [Test]
    public void Evaluate_Precedence_PowerOverMultiplication()
    {
        AssertEvaluation("2 * 3 ^ 3", 54);
    }

    [Test]
    public void Evaluate_Precedence_PowerOverAddition()
    {
        AssertEvaluation("10 + 2 ^ 3", 18);
    }

    [Test]
    public void Evaluate_Precedence_MixedThreeLevels()
    {
        AssertEvaluation("1 + 2 * 3 ^ 2", 19);
    }

    [Test]
    public void Evaluate_Parentheses_OverridePrecedence()
    {
        AssertEvaluation("(100 + 200) * 3", 900);
    }

    [Test]
    public void Evaluate_Parentheses_ForceAdditionBeforeMultiply()
    {
        AssertEvaluation("(5 + 3) * (2 + 4)", 48);
    }

    [Test]
    public void Evaluate_Parentheses_InMiddleOfChain()
    {
        AssertEvaluation("10 + (4 * 5) - 8", 22);
    }

    [Test]
    public void Evaluate_Parentheses_WithPower()
    {
        AssertEvaluation("2 ^ (2 + 3)", 32);
    }

    [Test]
    public void Evaluate_Parentheses_NestedWithDivision()
    {
        AssertEvaluation("(100 / (10 / 2)) + 5", 25);
    }

    [Test]
    public void Evaluate_ComplexMixed_GameLike()
    {
        AssertEvaluation("100 / 5 + 3 * 4 - 2 ^ 3", 24);
    }

    [Test]
    public void Evaluate_ComplexMixed_WithParentheses()
    {
        AssertEvaluation("(10 - 4) * 3 + 100 / 25", 22);
    }

    [Test]
    public void Evaluate_ComplexMixed_MultipleParentheses()
    {
        AssertEvaluation("(2 + 3) * (4 + 1) - (100 / 10)", 15);
    }
}