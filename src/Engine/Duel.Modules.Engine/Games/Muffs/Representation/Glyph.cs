namespace Duel.Modules.Engine.Games.Muffs.Representation;

public abstract record Glyph;

public abstract record Literal : Glyph;

public abstract record Operator : Glyph;

public enum OperatorType : byte
{
    Addition,
    Subtraction,
    Multiplication,
    Division,
    Modulo,
    Power,
    Negation,
    AbsoluteValue,
    Factorial,
    SquareRoot
}

