namespace Duel.Modules.Engine.Games.Muffs.Representation;

public abstract record Unary(Glyph Operand) : Operator;

public sealed record Negate(Glyph Operand) : Unary(Operand);

public sealed record Absolute(Glyph Operand) : Unary(Operand);

public sealed record SquareRoot(Glyph Operand) : Unary(Operand);

public sealed record Factorial(Glyph Operand) : Unary(Operand);