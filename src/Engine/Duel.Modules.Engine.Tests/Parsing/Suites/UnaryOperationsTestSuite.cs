using Duel.Modules.Engine.Games.Muffs.AST;

namespace Duel.Modules.Engine.Tests.Parsing.Suites;

public sealed class UnaryOperationsTestSuite : ExpressionParserTestSuite
{
    [Test]
    public void Negation_SimpleLiteral_ReturnsNegationNode()
    {
        var expression = "minus(5)";

        var result = GetExpression(expression);

        Assert.That(result, Is.InstanceOf<Negation>());
        var negation = result.As<Negation>();
        Assert.That(negation.Operand, Is.InstanceOf<Constant>());
        Assert.That(negation.Operand.As<Constant>().Value, Is.EqualTo(5));
    }

    [Test]
    public void Abs_SimpleLiteral_ReturnsAbsNode()
    {
        var expression = "abs(10)";

        var result = GetExpression(expression);

        Assert.That(result, Is.InstanceOf<Abs>());
        var abs = result.As<Abs>();
        Assert.That(abs.Operand, Is.InstanceOf<Constant>());
        Assert.That(abs.Operand.As<Constant>().Value, Is.EqualTo(10));
    }

    [Test]
    public void Sqrt_SimpleLiteral_ReturnsSqrtNode()
    {
        var expression = "sqrt(16)";

        var result = GetExpression(expression);

        Assert.That(result, Is.InstanceOf<SquareRoot>());
        var sqrt = result.As<SquareRoot>();
        Assert.That(sqrt.Operand, Is.InstanceOf<Constant>());
        Assert.That(sqrt.Operand.As<Constant>().Value, Is.EqualTo(16));
    }

    [Test]
    public void Factorial_SimpleLiteral_ReturnsFactorialNode()
    {
        var expression = "factorial(5)";

        var result = GetExpression(expression);

        Assert.That(result, Is.InstanceOf<Factorial>());
        var factorial = result.As<Factorial>();
        Assert.That(factorial.Operand, Is.InstanceOf<Constant>());
        Assert.That(factorial.Operand.As<Constant>().Value, Is.EqualTo(5));
    }

    [Test]
    public void Unary_WithBinaryExpression_ParsesCorrectly()
    {
        var expression = "abs(5 - 10)";

        var result = GetExpression(expression);

        Assert.That(result, Is.InstanceOf<Abs>());
        var abs = result.As<Abs>();
        Assert.That(abs.Operand, Is.InstanceOf<Subtraction>());
    }

    [Test]
    public void Unary_Nested_ParsesCorrectly()
    {
        var expression = "abs(minus(7))";

        var result = GetExpression(expression);

        Assert.That(result, Is.InstanceOf<Abs>());
        var abs = result.As<Abs>();
        Assert.That(abs.Operand, Is.InstanceOf<Negation>());
        var negation = abs.Operand.As<Negation>();
        Assert.That(negation.Operand, Is.InstanceOf<Constant>());
        Assert.That(negation.Operand.As<Constant>().Value, Is.EqualTo(7));
    }

    [Test]
    public void Unary_AsPartOfBinaryExpression_ParsesCorrectly()
    {
        var expression = "minus(5) + 10";

        var result = GetExpression(expression);

        Assert.That(result, Is.InstanceOf<Addition>());
        var addition = result.As<Addition>();
        Assert.That(addition.Left, Is.InstanceOf<Negation>());
        Assert.That(addition.Right, Is.InstanceOf<Constant>());
    }

    [Test]
    public void MultipleUnaryFunctions_InExpression_ParsesCorrectly()
    {
        var expression = "abs(minus(3)) * sqrt(16)";

        var result = GetExpression(expression);

        Assert.That(result, Is.InstanceOf<Multiplication>());
        var multiplication = result.As<Multiplication>();
        Assert.That(multiplication.Left, Is.InstanceOf<Abs>());
        Assert.That(multiplication.Right, Is.InstanceOf<SquareRoot>());
    }
}

