using Duel.Core.Games.Muffs.AST;

namespace Duel.Core.Games.Muffs.Generating;

public readonly record struct ConstantSettings(int Min, int Max);

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
        Depth: new(),
        Exponent: new(),
        Constant: new(),
        Add: new(),
        Subtract: new(),
        Multiply: new(),
        Divide: new(),
        Power: new()
    );

    public static readonly ExpressionSettings Medium = new(
        Depth: new(),
        Exponent: new(),
        Constant: new(),
        Add: new(),
        Subtract: new(),
        Multiply: new(),
        Divide: new(),
        Power: new()   
    );

    public static readonly ExpressionSettings Hard = new(
        Depth: new(),
        Exponent: new(),
        Constant: new(),
        Add: new(),
        Subtract: new(),
        Multiply: new(),
        Divide: new(),
        Power: new()  
    );
}