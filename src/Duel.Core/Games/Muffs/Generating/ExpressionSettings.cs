namespace Duel.Core.Games.Muffs.Generating;

public readonly record struct ConstantSettings(int Minimum, int Maximum)
{
    public static ConstantSettings From(int minimum, int maximum)
    {
        return new ConstantSettings(minimum, maximum);
    }
}

public readonly record struct OperatorSettings(bool Enabled, double Weight)
{
    public static OperatorSettings From(bool enabled, double weight)
    {
        return new OperatorSettings(enabled, weight);
    }

    public double GetWeight()
    {
        return Enabled ? Weight : 0d;
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
        Add: OperatorSettings.From(enabled: true, weight: 1.0d),
        Subtract: OperatorSettings.From(enabled: true, weight: 1.0d),
        Multiply: OperatorSettings.From(enabled: true, weight: 0.5d),
        Divide: OperatorSettings.From(enabled: true, weight: 0.3d),
        Power: OperatorSettings.From(enabled: true, weight: 0.1d)
    );

    public static readonly ExpressionSettings Medium = new(
        Depth: ConstantSettings.From(minimum: 2, maximum: 3),
        Constant: ConstantSettings.From(minimum: 1, maximum: 15),
        Exponent: ConstantSettings.From(minimum: 2, maximum: 4),
        Add: OperatorSettings.From(enabled: true, weight: 1.0d),
        Subtract: OperatorSettings.From(enabled: true, weight: 1.0d),
        Multiply: OperatorSettings.From(enabled: true, weight: 0.8d),
        Divide: OperatorSettings.From(enabled: true, weight: 0.6d),
        Power: OperatorSettings.From(enabled: true, weight: 0.3d)
    );

    public static readonly ExpressionSettings Hard = new(
        Depth: ConstantSettings.From(minimum: 3, maximum: 4),
        Constant: ConstantSettings.From(minimum: 1, maximum: 20),
        Exponent: ConstantSettings.From(minimum: 2, maximum: 5),
        Add: OperatorSettings.From(enabled: true, weight: 1.0d),
        Subtract: OperatorSettings.From(enabled: true, weight: 1.0d),
        Multiply: OperatorSettings.From(enabled: true, weight: 1.0d),
        Divide: OperatorSettings.From(enabled: true, weight: 0.8d),
        Power: OperatorSettings.From(enabled: true, weight: 0.5d)
    );
}