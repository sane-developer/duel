using Duel.Core.Games.Muffs.AST;

namespace Duel.Core.Games.Muffs.Generating;

public readonly record struct ConstantSettings(int Minimum, int Maximum);

public readonly record struct OperatorSettings(Operator.Code Code, float Weight);

public readonly record struct ExpressionSettings(
    ConstantSettings Depth,
    ConstantSettings Constant,
    ConstantSettings Exponent,
    OperatorSettings Add,
    OperatorSettings Subtract,
    OperatorSettings Multiply,
    OperatorSettings Divide,
    OperatorSettings Power
);

public static class ExpressionSettingsRegistry
{
    public static readonly ExpressionSettings Easy = new(
        Depth: new(Minimum: 1, Maximum: 2),
        Constant: new(Minimum: 1, Maximum: 10),
        Exponent: new(Minimum: 2, Maximum: 3),
        Add: new(Operator.Code.Add, Weight: 1.0f),
        Subtract: new(Operator.Code.Subtract, Weight: 1.0f),
        Multiply: new(Operator.Code.Multiply, Weight: 0.5f),
        Divide: new(Operator.Code.Divide, Weight: 0.3f),
        Power: new(Operator.Code.Power, Weight: 0.1f)
    );

    public static readonly ExpressionSettings Medium = new(
        Depth: new(Minimum: 2, Maximum: 3),
        Constant: new(Minimum: 1, Maximum: 15),
        Exponent: new(Minimum: 2, Maximum: 4),
        Add: new(Operator.Code.Add, Weight: 1.0f),
        Subtract: new(Operator.Code.Subtract, Weight: 1.0f),
        Multiply: new(Operator.Code.Multiply, Weight: 0.8f),
        Divide: new(Operator.Code.Divide, Weight: 0.6f),
        Power: new(Operator.Code.Power, Weight: 0.3f)
    );

    public static readonly ExpressionSettings Hard = new(
        Depth: new(Minimum: 3, Maximum: 4),
        Constant: new(Minimum: 1, Maximum: 20),
        Exponent: new(Minimum: 2, Maximum: 5),
        Add: new(Operator.Code.Add, Weight: 1.0f),
        Subtract: new(Operator.Code.Subtract, Weight: 1.0f),
        Multiply: new(Operator.Code.Multiply, Weight: 1.0f),
        Divide: new(Operator.Code.Divide, Weight: 0.8f),
        Power: new(Operator.Code.Power, Weight: 0.5f)
    );
}