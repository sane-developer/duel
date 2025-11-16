namespace Duel.Modules.Engine.Muffs.Glyphs;

public abstract record Glyph(GlyphType Type)
{
    public BinaryOperator AsBinary() => (BinaryOperator) this;

    public UnaryOperator AsUnary() => (UnaryOperator) this;

    public Number AsNumber() => (Number) this;
};

public sealed record BinaryOperator(GlyphType Type) : Glyph(Type)
{
    public Glyph Lhs { get; set; }

    public Glyph Rhs { get; set; }
}

public sealed record UnaryOperator(GlyphType Type) : Glyph(Type)
{
    public Glyph Operand { get; set; }
}

public sealed record Number(int Value) : Glyph(GlyphType.Number);