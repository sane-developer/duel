using Duel.Modules.Engine.Muffs.Glyphs;

namespace Duel.Modules.Engine.Muffs.Compositions;

public record Composition(GlyphType OperatorType, int Result)
{
    public static readonly Composition Null = new(GlyphType.Null, int.MaxValue);
};

public sealed record BinaryComposition(GlyphType OperatorType, int Lhs, int Rhs, int Result) : Composition(OperatorType, Result);

public sealed record UnaryComposition(GlyphType OperatorType, int Operand, int Result) : Composition(OperatorType, Result);