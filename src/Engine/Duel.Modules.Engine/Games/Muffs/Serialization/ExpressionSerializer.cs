using Duel.Modules.Engine.Games.Muffs.Representation;

namespace Duel.Modules.Engine.Games.Muffs.Serialization;

public static class ExpressionSerializer
{
    public static string Serialize(Glyph root) => root switch
    {
        Number number => number.Value.ToString(),
        Add add => $"({Serialize(add.Lhs)} + {Serialize(add.Rhs)})",
        Subtract subtract => $"({Serialize(subtract.Lhs)} - {Serialize(subtract.Rhs)})",
        Multiply multiply => $"({Serialize(multiply.Lhs)} * {Serialize(multiply.Rhs)})",
        Divide divide => $"({Serialize(divide.Lhs)} / {Serialize(divide.Rhs)})",
        Modulo modulo => $"({Serialize(modulo.Lhs)} % {Serialize(modulo.Rhs)})",
        Power power => $"({Serialize(power.Lhs)} ^ {Serialize(power.Rhs)})",
        Negate negate => $"-({Serialize(negate.Operand)})",
        Absolute absolute => $"|{Serialize(absolute.Operand)}|",
        Factorial factorial => $"{Serialize(factorial.Operand)}!",
        SquareRoot squareRoot => $"√{Serialize(squareRoot.Operand)}",
        _ => Situation.Unreachable<string>()
    };
}

