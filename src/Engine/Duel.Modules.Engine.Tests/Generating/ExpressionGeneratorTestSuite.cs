using Duel.Modules.Engine.Games.Muffs.AST;
using Duel.Modules.Engine.Games.Muffs.Generating;
using Duel.Modules.Engine.Games.Muffs.Generating.Strategies;

namespace Duel.Modules.Engine.Tests.Generating;

public abstract class ExpressionGeneratorTestSuite
{
    private static readonly ConstantSettings ConstantSettings = ConstantSettings.From(
        minimum: 1, 
        maximum: 10
    );

    private static readonly ConstantSettings ExponentSettings = ConstantSettings.From(
        minimum: 2, 
        maximum: 3
    );

    protected static Expression GetExpression(ExpressionSettings settings, Random rng)
    {
        var strategy = new RandomExpressionStrategy(settings, rng);
        
        var generator = new ExpressionGenerator(strategy);

        return generator.Generate();
    }

    protected static ExpressionSettings CreateConstantOnlySettings()
    {
        return new ExpressionSettings(
            Depth: ConstantSettings.From(minimum: 0, maximum: 0),
            Constant: ConstantSettings,
            Exponent: ExponentSettings,
            Add: OperatorSettings.From(enabled: false, weight: 0f),
            Subtract: OperatorSettings.From(enabled: false, weight: 0f),
            Multiply: OperatorSettings.From(enabled: false, weight: 0f),
            Divide: OperatorSettings.From(enabled: false, weight: 0f),
            Power: OperatorSettings.From(enabled: false, weight: 0f)
        );
    }

    protected static ExpressionSettings CreateSingleOperatorSettings(Operator.Code code)
    {
        return new ExpressionSettings(
            Depth: ConstantSettings.From(minimum: 1, maximum: 2),
            Constant: ConstantSettings,
            Exponent: ExponentSettings,
            Add: OperatorSettings.From(enabled: code is Operator.Code.Add, weight: 1.0f),
            Subtract: OperatorSettings.From(enabled: code is Operator.Code.Subtract, weight: 1.0f),
            Multiply: OperatorSettings.From(enabled: code is Operator.Code.Multiply, weight: 1.0f),
            Divide: OperatorSettings.From(enabled: code is Operator.Code.Divide, weight: 1.0f),
            Power: OperatorSettings.From(enabled: code is Operator.Code.Power, weight: 1.0f)
        );
    }

    protected static void IsValidExponent(int exponent)
    {
        Assert.That(exponent, Is.InRange(ExponentSettings.Minimum, ExponentSettings.Maximum), "Exponent value is out of range.");
    }

    protected static void IsValidDivisor(int dividend, int divisor)
    {
        Assert.That(dividend % divisor, Is.EqualTo(0), $"Division {dividend} / {divisor} must be exact.");
    }

    protected static void IsValidConstant(Expression expression)
    {
        Assert.That(expression, Is.TypeOf<Constant>(), "Expression must be a Constant node.");

        var constant = expression.As<Constant>();

        Assert.That(constant.Value, Is.InRange(ConstantSettings.Minimum, ConstantSettings.Maximum), "Constant value is out of range.");
    }

    protected static T IsValidBinary<T>(Expression expression) where T : Binary
    {
        Assert.That(expression, Is.TypeOf<T>(), $"Expression must be a Binary node.");

        var binary = expression.As<Binary>();

        IsValidConstant(binary.Left);

        IsValidConstant(binary.Right);

        return binary.As<T>();
    }
}