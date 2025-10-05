namespace Duel.Core.Games.Muffs.Generating;

public readonly record struct ConstantSettings(int Minimum, int Maximum)
{
    public static ConstantSettings From(int minimum, int maximum)
    {
        return new ConstantSettings(minimum, maximum);
    }
}

public readonly record struct OperatorSettings(float Weight)
{
    public static OperatorSettings From(float weight)
    {
        return new OperatorSettings(weight);
    }
}

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

internal static class ExpressionSettingsRegistry
{
    public static readonly ExpressionSettings Easy = new(
        Depth: ConstantSettings.From(minimum: 1, maximum: 2),
        Constant: ConstantSettings.From(minimum: 1, maximum: 10),
        Exponent: ConstantSettings.From(minimum: 2, maximum: 3),
        Add: OperatorSettings.From(weight: 1.0f),
        Subtract: OperatorSettings.From(weight: 1.0f),
        Multiply: OperatorSettings.From(weight: 0.5f),
        Divide: OperatorSettings.From(weight: 0.3f),
        Power: OperatorSettings.From(weight: 0.1f)
    );

    public static readonly ExpressionSettings Medium = new(
        Depth: ConstantSettings.From(minimum: 2, maximum: 3),
        Constant: ConstantSettings.From(minimum: 1, maximum: 15),
        Exponent: ConstantSettings.From(minimum: 2, maximum: 4),
        Add: OperatorSettings.From(weight: 1.0f),
        Subtract: OperatorSettings.From(weight: 1.0f),
        Multiply: OperatorSettings.From(weight: 0.8f),
        Divide: OperatorSettings.From(weight: 0.6f),
        Power: OperatorSettings.From(weight: 0.3f)
    );

    public static readonly ExpressionSettings Hard = new(
        Depth: ConstantSettings.From(minimum: 3, maximum: 4),
        Constant: ConstantSettings.From(minimum: 1, maximum: 20),
        Exponent: ConstantSettings.From(minimum: 2, maximum: 5),
        Add: OperatorSettings.From(weight: 1.0f),
        Subtract: OperatorSettings.From(weight: 1.0f),
        Multiply: OperatorSettings.From(weight: 1.0f),
        Divide: OperatorSettings.From(weight: 0.8f),
        Power: OperatorSettings.From(weight: 0.5f)
    );
}