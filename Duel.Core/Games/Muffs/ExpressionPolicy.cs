namespace Duel.Core.Games.Muffs;

public readonly record struct ExpressionPolicy(
    ushort MaxDepth,
    ushort MaxAbsoluteValue,
    ushort MaxDividendValue,
    ushort MaxComponentValue,
    ushort MaxMultiplierValue,
    ushort MaxExponentValue,
    ushort MaxExponentBaseValue
);