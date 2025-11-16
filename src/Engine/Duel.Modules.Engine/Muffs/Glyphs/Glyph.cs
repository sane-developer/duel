namespace Duel.Modules.Engine.Muffs.Glyphs;

public abstract record Glyph(GlyphType Type)
{
    public BinaryOperator AsBinary() => (BinaryOperator) this;

    public UnaryOperator AsUnary() => (UnaryOperator) this;

    public Number AsNumber() => (Number) this;
};

public sealed record BinaryOperator(GlyphType Type, Glyph Lhs, Glyph Rhs) : Glyph(Type)
{
    public static BinaryOperator From(GlyphType type, Glyph lhs, Glyph rhs)
    {
        return new BinaryOperator(type, lhs, rhs);
    }
}

public sealed record UnaryOperator(GlyphType Type, Glyph Operand) : Glyph(Type)
{
    public static UnaryOperator From(GlyphType type, Glyph operand)
    {
        return new UnaryOperator(type, operand);
    }
}

public sealed record Number(int Value) : Glyph(GlyphType.Number)
{
    public static Number From(int value)
    {
        return new Number(value);
    }
}