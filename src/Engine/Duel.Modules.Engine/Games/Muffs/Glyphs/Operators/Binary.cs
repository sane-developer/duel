namespace Duel.Modules.Engine.Games.Muffs.Glyphs.Operators;

public abstract record Binary(Glyph Lhs, Glyph Rhs) : Operator;

public sealed record Add(Glyph Lhs, Glyph Rhs) : Binary(Lhs, Rhs);

public sealed record Subtract(Glyph Lhs, Glyph Rhs) : Binary(Lhs, Rhs);

public sealed record Multiply(Glyph Lhs, Glyph Rhs) : Binary(Lhs, Rhs);

public sealed record Divide(Glyph Lhs, Glyph Rhs) : Binary(Lhs, Rhs);

public sealed record Modulo(Glyph Lhs, Glyph Rhs) : Binary(Lhs, Rhs);

public sealed record Power(Glyph Lhs, Glyph Rhs) : Binary(Lhs, Rhs);